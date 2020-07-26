namespace RTCV.Plugins.HexEditor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using RTCV.NetCore;
    using RTCV.CorruptCore;
    using NLog;
    using NLog.Layouts;

    //Based on the Hex Editor from Bizhawk, available under MIT.
    //https://github.com/tasvideos/bizhawk
    public partial class HexEditor : Form
    {
        private int fontWidth;
        private int fontHeight;

        private readonly char[] _nibbles = { 'G', 'G', 'G', 'G', 'G', 'G', 'G', 'G' };    // G = off 0-9 & A-F are acceptable values
        private readonly List<long> _secondaryHighlightedAddresses = new List<long>();

        private readonly Dictionary<int, char> _textTable = new Dictionary<int, char>();

        private int _rowsVisible;
        private int _numDigits = 4;
        private string _numDigitsStr = "{0:X4}";
        private string _digitFormatString = "{0:X2}";
        private long _addressHighlighted = -1;
        private long _addressOver = -1;

        private long _maxRow;

        private MemoryInterface _domain = new NullMemoryInterface();

        private long _row;
        private long _addr;
        private string _findStr = "";
        private bool _mouseIsDown;
        private HexFind _hexFind = new HexFind();

        private string LastDomain { get; set; }

        private bool SwapBytes { get; set; }

        private bool BigEndian { get; set; }

        private int DataSize { get; set; }

        private Dictionary<string, MemoryInterface> AllDomains => MemoryDomains.AllMemoryInterfaces;
        public volatile bool UpdateOnStep = true;
        public volatile bool HideOnClose = true;

        Logger logger = NLog.LogManager.GetCurrentClassLogger();


        public HexEditor()
        {
            DataSize = 1;

            var font = new Font("Courier New", 8);

            // Measure the font. There seems to be some extra horizontal padding on the first
            // character so we'll see how much the width increases on the second character.
            var fontSize1 = TextRenderer.MeasureText("0", font);
            var fontSize2 = TextRenderer.MeasureText("00", font);
            fontWidth = fontSize2.Width - fontSize1.Width;
            fontHeight = fontSize1.Height;

            InitializeComponent();
            this.FormClosing += HexEditor_FormClosing;
            this.VisibleChanged += HexEditor_VisibleChanged;

            AddressesLabel.BackColor = Color.Transparent;
            //LoadConfigSettings();
            SetHeader();
            //Closing += (o, e) => SaveConfigSettings();

            Header.Font = font;
            AddressesLabel.Font = font;
            AddressLabel.Font = font;

            Restart();

            CorruptCore.StepActions.StepEnd += (o, e) =>
            {
                if (this.Visible)
                    UpdateValues();
            };
            CorruptCore.RtcCore.GameClosed += (o, e) =>
            {
                if (e.FullyClosed)
                {
                    HideOnClose = false;
                    this.Close();
                }
                else if (this.Visible)
                    Restart();
            };
            CorruptCore.RtcCore.LoadGameDone += (o, e) =>
            {
                if (this.Visible)
                    Restart();
            };


            if (!Params.IsParamSet("HEXEDITOR_WARNING"))
            {
                MessageBox.Show("While the hex editor works fine for most people, on some systems it'll cause random crashes.\n" +
                                "If you find yourself experiencing weird emulator issues, try disabling this plugin.\n\n" +
                                "This message will only appear once.");
                Params.SetParam("HEXEDITOR_WARNING");
            }
        }

        private void HexEditor_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                Restart();
                this.BringToFront();
                this.Activate();
            }
            else
            {
                _domain = new NullMemoryInterface();
            }
        }

        private void HexEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (HideOnClose)
            {
                this.Hide();
                _domain = new NullMemoryInterface();
                e.Cancel = true;
            }
        }

        private long? HighlightedAddress
        {
            get
            {
                if (_addressHighlighted >= 0)
                {
                    return _addressHighlighted;
                }

                return null; // Negative = no address highlighted
            }
        }

        #region API

        public bool UpdateBefore => false;

        public bool AskSaveChanges()
        {
            return true;
        }

        public async void UpdateValues()
        {
            await Task.Run(() => SyncObjectSingleton.FormExecute(() =>
            {
                try
                {
                    AddressesLabel.Text = GenerateMemoryViewString(true);
                    AddressLabel.Text = GenerateAddressString();
                }
                catch (Exception e)
                {
                    logger.Error(e, "Failed to UpdateValues() in hex editor.");
                }
            }));
        }

        private string _lastRom = "";

        public void Restart()
        {
            SyncObjectSingleton.FormExecute(() =>
            {
                if (AllDomains.Count == 0)
                {
                    _domain = new NullMemoryInterface();
                }
                else if (AllDomains.Any(x => x.Value.Name == _domain.Name))
                {
                    _domain = AllDomains[_domain.Name];
                }
                else
                {
                    _domain = AllDomains.Values.First();
                }

                SwapBytes = false;
                BigEndian = _domain.BigEndian;
                DataSize = _domain.WordSize;

                _maxRow = _domain.Size / 2;

                // Don't reset scroll bar if restarting the same rom
                if (_lastRom != (string)(AllSpec.VanguardSpec?[VSPEC.NAME] ?? "UNKNOWN"))
                {
                    _lastRom = (string)(AllSpec.VanguardSpec?[VSPEC.NAME] ?? "UNKNOWN");
                    ResetScrollBar();
                }
                else
                {
                    SetUpScrollBar();
                }

                SetDataSize(DataSize);
                SetHeader();

                UpdateValues();
                AddressLabel.Text = GenerateAddressString();
                this.Refresh();
            });
        }

        public byte[] ConvertTextToBytes(string str)
        {
            if (_textTable.Any())
            {
                var byteArr = new List<byte>();
                foreach (var chr in str)
                {
                    byteArr.Add((byte)_textTable.FirstOrDefault(kvp => kvp.Value == chr).Key);
                }

                return byteArr.ToArray();
            }

            return str.Select(Convert.ToByte).ToArray();
        }

        public byte[] ConvertHexStringToByteArray(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return new byte[0];
            }

            // TODO: Better method of handling this?
            if (str.Length % 2 == 1)
            {
                str += "0";
            }

            byte[] bytes = new byte[str.Length / 2];

            for (int i = 0; i < str.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(str.Substring(i, 2), 16);
            }

            return bytes;
        }

        public void FindNext(string value, bool wrap)
        {
            long found = -1;

            var search = value.Replace(" ", "").ToUpper();
            if (string.IsNullOrEmpty(search))
            {
                return;
            }

            var numByte = search.Length / 2;

            long startByte;
            if (_addressHighlighted == -1)
            {
                startByte = 0;
            }
            else if (_addressHighlighted >= (_domain.Size - 1 - numByte))
            {
                startByte = 0;
            }
            else
            {
                startByte = _addressHighlighted + DataSize;
            }

            byte[] searchBytes = ConvertHexStringToByteArray(search);
            for (var i = startByte; i < (_domain.Size - numByte); i++)
            {
                bool differenceFound = false;
                for (var j = 0; j < numByte; j++)
                {
                    if (_domain.PeekByte(i + j) != searchBytes[j])
                    {
                        differenceFound = true;
                        break;
                    }
                }

                if (!differenceFound)
                {
                    found = i;
                    break;
                }
            }

            if (found > -1)
            {
                HighlightSecondaries(search, found);
                GoToAddress(found);
                _findStr = search;
            }
            else if (wrap == false)
            {
                FindPrev(value, true); // Search the opposite direction if not found
            }

            _hexFind.Close();
        }

        public void FindPrev(string value, bool wrap)
        {
            long found = -1;

            var search = value.Replace(" ", "").ToUpper();
            if (string.IsNullOrEmpty(search))
            {
                return;
            }

            var numByte = search.Length / 2;

            long startByte;
            if (_addressHighlighted == -1)
            {
                startByte = _domain.Size - DataSize - numByte;
            }
            else
            {
                startByte = _addressHighlighted - 1;
            }

            byte[] searchBytes = ConvertHexStringToByteArray(search);
            for (var i = startByte; i >= 0; i--)
            {
                bool differenceFound = false;
                for (var j = 0; j < numByte; j++)
                {
                    if (_domain.PeekByte(i + j) != searchBytes[j])
                    {
                        differenceFound = true;
                        break;
                    }
                }

                if (!differenceFound)
                {
                    found = i;
                    break;
                }
            }

            if (found > -1)
            {
                HighlightSecondaries(search, found);
                GoToAddress(found);
                _findStr = search;
            }
            else if (wrap == false)
            {
                FindPrev(value, true); // Search the opposite direction if not found
            }

            _hexFind.Close();
        }

        #endregion

        private char Remap(byte val)
        {
            if (_textTable.Any())
            {
                if (_textTable.ContainsKey(val))
                {
                    return _textTable[val];
                }

                return '?';
            }
            else
            {
                if (val < ' ')
                {
                    return '.';
                }

                if (val >= 0x7F)
                {
                    return '.';
                }

                return (char)val;
            }
        }

        private static int GetNumDigits(long i)
        {
            if (i <= 0x10000)
            {
                return 4;
            }

            return i <= 0x1000000 ? 6 : 8;
        }

        private static char ForceCorrectKeyString(char keycode)
        {
            return keycode;
        }

        private static bool IsHexKeyCode(char key)
        {
            if (key >= '0' && key <= '9') // 0-9
            {
                return true;
            }

            if (key >= 'a' && key <= 'f') // A-F
            {
                return true;
            }

            if (key >= 'A' && key <= 'F') // A-F
            {
                return true;
            }

            return false;
        }

        private void HexEditor_Load(object sender, EventArgs e)
        {
            Restart();
            this.BringToFront();
            this.Activate();
        }

        // TODO: rename me
        private void SaveConfigSettings()
        {
            if (_hexFind.IsHandleCreated || !_hexFind.IsDisposed)
            {
                _hexFind.Close();
            }
        }

        private string GenerateAddressString()
        {
            var addrStr = new StringBuilder();

            for (var i = 0; i < _rowsVisible; i++)
            {
                _row = i + HexScrollBar.Value;
                _addr = _row << 4;
                if (_addr >= _domain.Size)
                {
                    break;
                }

                if (_numDigits == 4)
                {
                    addrStr.Append("    "); // Hack to line things up better between 4 and 6
                }
                else if (_numDigits == 6)
                {
                    addrStr.Append("  ");
                }

                addrStr.AppendLine($"{string.Format("{0:X" + _numDigits + "}", _addr)} |");
            }

            return addrStr.ToString();
        }

        private bool MakeValue(int dataSize, long address, out int value)
        {
            value = 0;
            try
            {
                switch (dataSize)
                {
                    case 1:
                        value = _domain.PeekByte(address);
                        break;
                    case 2:
                        value = BitConverter.ToInt16(_domain.PeekBytes(address, address + 2, !SwapBytes), 0);
                        break;
                    case 4:
                        value = BitConverter.ToInt16(_domain.PeekBytes(address, address + 4, !SwapBytes), 0);
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Unable to MakeValue");
                return false;
            }

            return true;
        }

        private string GenerateMemoryViewString(bool forWindow)
        {
            var rowStr = new StringBuilder();

            for (var i = 0; i < _rowsVisible; i++)
            {
                _row = i + HexScrollBar.Value;
                _addr = _row << 4;
                if (_addr >= _domain.Size)
                {
                    break;
                }

                for (var j = 0; j < 16; j += DataSize)
                {
                    if (_addr + j + DataSize <= _domain.Size)
                    {
                        int t_val = 0;
                        int t_next = 0;
                        for (int k = 0; k < DataSize; k++)
                        {
                            if (!MakeValue(1, _addr + j + k, out t_next))
                            {
                                return "ERROR";
                            }

                            if (SwapBytes)
                            {
                                t_val += (t_next << (k * 8));
                            }
                            else
                            {
                                t_val += (t_next << ((DataSize - k - 1) * 8));
                            }
                        }

                        rowStr.AppendFormat(_digitFormatString, t_val);
                    }
                    else
                    {
                        for (var t = 0; t < DataSize; t++)
                        {
                            rowStr.Append("  ");
                        }

                        rowStr.Append(' ');
                    }
                }

                rowStr.Append("| ");
                for (var k = 0; k < 16; k++)
                {
                    if (_addr + k < _domain.Size)
                    {
                        byte b = MakeByte(_addr + k);
                        char c = Remap(b);
                        rowStr.Append(c);
                        //winforms will be using these as escape codes for hotkeys
                        if (forWindow)
                        {
                            if (c == '&')
                            {
                                rowStr.Append('&');
                            }
                        }
                    }
                }

                rowStr.AppendLine();
            }

            return rowStr.ToString();
        }

        private byte MakeByte(long address)
        {
            return _domain.PeekByte(address);
        }

        public void SetDomain(MemoryInterface mi)
        {
            SetMemoryDomain(mi.Name);
        }

        private void SetMemoryDomain(string name)
        {
            if (!AllDomains.TryGetValue(name, out _domain))
            {
                _domain = new NullMemoryInterface();
            }

            SwapBytes = false;
            BigEndian = _domain.BigEndian;
            _maxRow = _domain.Size / 2;
            SetUpScrollBar();
            if (0 >= HexScrollBar.Minimum && 0 <= HexScrollBar.Maximum)
            {
                HexScrollBar.Value = 0;
            }

            AddressesLabel.ForeColor = SystemColors.ControlText;

            if (HighlightedAddress >= _domain.Size
                || (_secondaryHighlightedAddresses.Any() && _secondaryHighlightedAddresses.Max() >= _domain.Size))
            {
                _addressHighlighted = -1;
                _secondaryHighlightedAddresses.Clear();
            }

            UpdateGroupBoxTitle();
            SetHeader();
            UpdateValues();
            LastDomain = _domain.Name;
            this.Refresh();
        }

        private void UpdateGroupBoxTitle()
        {
            var addressesString = "0x" + $"{_domain.Size / DataSize:X8}".TrimStart('0');
            MemoryViewerBox.Text = $"{AllSpec.VanguardSpec?[VSPEC.NAME] ?? "UNKNOWN"} {_domain} -  {addressesString} addresses";
        }

        private void ClearNibbles()
        {
            for (var i = 0; i < 8; i++)
            {
                _nibbles[i] = 'G';
            }
        }

        public void GoToAddress(long address)
        {
            if (address < 0)
            {
                address = 0;
            }

            if (address >= _domain.Size)
            {
                address = _domain.Size - DataSize;
            }

            SetHighlighted(address);
            ClearNibbles();
            UpdateValues();
            MemoryViewerBox.Refresh();
            AddressLabel.Text = GenerateAddressString();
        }

        private void SetHighlighted(long address)
        {
            if (address < 0)
            {
                address = 0;
            }

            if (address >= _domain.Size)
            {
                address = _domain.Size - DataSize;
            }

            if (!IsVisible(address))
            {
                var value = (address / 16) - _rowsVisible + 1;
                if (value < 0)
                {
                    value = 0;
                }

                HexScrollBar.Value = (int)value; // This will fail on a sufficiently large domain
            }

            _addressHighlighted = address;
            _addressOver = address;
            ClearNibbles();
            UpdateFormText();
        }

        private void UpdateFormText()
        {
            Text = "Hex Editor";
            if (_addressHighlighted >= 0)
            {
                Text += " - Editing Address 0x" + string.Format(_numDigitsStr, _addressHighlighted);
                if (_secondaryHighlightedAddresses.Any())
                {
                    Text += $" (Selected 0x{_secondaryHighlightedAddresses.Count + (_secondaryHighlightedAddresses.Contains(_addressHighlighted) ? 0 : 1):X})";
                }
            }
        }

        private bool IsVisible(long address)
        {
            var i = address >> 4;
            return i >= HexScrollBar.Value && i < (_rowsVisible + HexScrollBar.Value);
        }

        private void SetHeader()
        {
            switch (DataSize)
            {
                case 1:
                    Header.Text = "         0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F";
                    break;
                case 2:
                    Header.Text = "         0    2    4    6    8    A    C    E";
                    break;
                case 4:
                    Header.Text = "         0        4        8        C";
                    break;
            }

            _numDigits = GetNumDigits(_domain.Size);
            _numDigitsStr = $"{{0:X{_numDigits}}}  ";
        }

        private void SetDataSize(int size)
        {
            if (size == 1 || size == 2 || size == 4)
            {
                DataSize = size;
                _digitFormatString = $"{{0:X{DataSize * 2}}} ";
                SetHeader();
                UpdateGroupBoxTitle();
                UpdateValues();
                _secondaryHighlightedAddresses.Clear();
            }
        }

        private void SaveFileBinary(string path)
        {
            var file = new FileInfo(path);
            using (var binWriter = new BinaryWriter(File.Open(file.FullName, FileMode.Create)))
            {
                for (var i = 0; i < _domain.Size; i++)
                {
                    binWriter.Write(_domain.PeekByte(i));
                }
            }
        }

        private string GetSaveFileFilter()
        {
            return "Binary (*.bin)|*.bin|All Files|*.*";
        }

        private void ResetScrollBar()
        {
            HexScrollBar.Value = 0;
            SetUpScrollBar();
            Refresh();
        }

        private void SetUpScrollBar()
        {
            _rowsVisible = (MemoryViewerBox.Height - (fontHeight * 2) - (fontHeight / 2)) / fontHeight;
            var totalRows = (int)((_domain.Size + 15) / 16);

            if (totalRows < _rowsVisible)
            {
                _rowsVisible = totalRows;
            }

            HexScrollBar.Maximum = totalRows - 1;
            HexScrollBar.LargeChange = _rowsVisible;
            HexScrollBar.Visible = totalRows > _rowsVisible;

            AddressLabel.Text = GenerateAddressString();
        }

        private long GetPointedAddress(int x, int y)
        {
            long address;

            // Scroll value determines the first row
            long i = HexScrollBar.Value;
            var rowoffset = y / fontHeight;
            i += rowoffset;
            int colWidth = DataSize * 2 + 1;

            var column = x / (fontWidth * colWidth);

            var innerOffset = AddressesLabel.Location.X - AddressLabel.Location.X + AddressesLabel.Margin.Left;
            var start = GetTextOffset() - innerOffset;
            if (x > start)
            {
                column = (x - start) / (fontWidth * DataSize);
            }

            if (i >= 0 && i <= _maxRow && column >= 0 && column < (16 / DataSize))
            {
                address = (i * 16) + (column * DataSize);
            }
            else
            {
                address = -1;
            }

            return address;
        }

        private void DoShiftClick()
        {
            if (_addressOver >= 0 && _addressOver < _domain.Size)
            {
                _secondaryHighlightedAddresses.Clear();
                if (_addressOver < _addressHighlighted)
                {
                    for (var x = _addressOver; x < _addressHighlighted; x += DataSize)
                    {
                        _secondaryHighlightedAddresses.Add(x);
                    }
                }
                else if (_addressOver > _addressHighlighted)
                {
                    for (var x = _addressHighlighted + DataSize; x <= _addressOver; x += DataSize)
                    {
                        _secondaryHighlightedAddresses.Add(x);
                    }
                }

                if (!IsVisible(_addressOver))
                {
                    var value = (_addressOver / 16) + 1 - ((_addressOver / 16) < HexScrollBar.Value ? 1 : _rowsVisible);
                    if (value < 0)
                    {
                        value = 0;
                    }

                    HexScrollBar.Value = (int)value; // This will fail on a sufficiently large domain
                }
            }
        }

        private void ClearHighlighted()
        {
            _addressHighlighted = -1;
            UpdateFormText();
            MemoryViewerBox.Refresh();
        }

        private Point GetAddressCoordinates(long address)
        {
            var extra = (address % DataSize) * fontWidth * 2;
            var xOffset = AddressesLabel.Location.X + fontWidth / 2 - 2;
            var yOffset = AddressesLabel.Location.Y;

            return new Point(
                (int)((((address % 16) / DataSize) * (fontWidth * (DataSize * 2 + 1))) + xOffset + extra),
                (int)((((address / 16) - HexScrollBar.Value) * fontHeight) + yOffset)
                );
        }

        // TODO: rename this, but it is a hack work around for highlighting misaligned addresses that result from highlighting on in a smaller data size and switching size
        private bool NeedsExtra(long val)
        {
            return val % DataSize > 0;
        }

        private int GetTextOffset()
        {
            int start = (16 / DataSize) * fontWidth * (DataSize * 2 + 1);
            start += AddressesLabel.Location.X + fontWidth / 2;
            start += fontWidth * 2;
            return start;
        }

        private long GetTextX(long address)
        {
            return GetTextOffset() + ((address % 16) * fontWidth);
        }

        private bool HasNibbles()
        {
            return _nibbles.Any(x => x != 'G');
        }

        private string MakeNibbles()
        {
            var str = "";
            for (var x = 0; x < (DataSize * 2); x++)
            {
                if (_nibbles[x] != 'G')
                {
                    str += _nibbles[x];
                }
                else
                {
                    break;
                }
            }

            return str;
        }

        private void AddToSecondaryHighlights(long address)
        {
            if (address >= 0 && address < _domain.Size && !_secondaryHighlightedAddresses.Contains(address))
            {
                _secondaryHighlightedAddresses.Add(address);
            }
        }

        // TODO: obsolete me
        private void PokeWord(long address, byte _1, byte _2)
        {
            if (BigEndian)
            {
                _domain.PokeByte(address, _2);
                _domain.PokeByte(address + 1, _1);
            }
            else
            {
                _domain.PokeByte(address, _1);
                _domain.PokeByte(address + 1, _2);
            }
        }

        private void CreateVMDFromSelectedMenuItem_Click(object sender, EventArgs e)
        {
            if (HighlightedAddress == null)
            {
                return;
            }

            List<long> allAddresses = new List<long>() { HighlightedAddress.Value };
            allAddresses.AddRange(_secondaryHighlightedAddresses);
            CreateVmdFromSelected(_domain.Name, allAddresses, DataSize);

            MemoryViewerBox.Refresh();
        }

        public static void CreateVmdFromSelected(string domain, List<long> allAddresses, int wordSize)
        {
            int allAddrCount = allAddresses.Count;
            if (wordSize > 1) //fills the gap caused by address spacing
            {
                for (int addrPos = 0; addrPos < allAddrCount; addrPos++)
                {
                    for (int addedCount = 1; addedCount < wordSize; addedCount++)
                    {
                        long newAddr = allAddresses[addrPos] + addedCount;
                        allAddresses.Add(newAddr);
                    }
                }
            }

            var ordered = allAddresses.OrderBy(it => it).ToArray();

            bool contiguous = true;
            long? lastAddress = null;
            int i = 0;

            foreach (long item in ordered)
            {
                if (lastAddress != null) //not the first one
                {
                    if (i != (ordered.Length - 1)) //not the last one
                    {
                        if (item != lastAddress.Value + 1) //checks expected address
                        {
                            contiguous = false;
                        }
                    }
                }

                lastAddress = item;
                i++;
            }

            string ToHexString(long n)
            {
                return $"{n:X}";
            }

            string text;
            if (contiguous)
            {
                text = $"{ToHexString(ordered[0])}-{ToHexString(ordered[ordered.Length - 1])}";
            }
            else
            {
                text = string.Join("\n", ordered.Select(it => ToHexString(it)));
            }
            CreateVmdText(domain, text);
        }

        internal static void CreateVmdText(string domain, string text)
        {   //Sends text to the VMD Generator and trigger generation
            LocalNetCoreRouter.Route(NetcoreCommands.UI, NetcoreCommands.REMOTE_GENERATEVMDTEXT, new object[] { domain, text }, false);
        }

        private void IncrementAddress(long address)
        {
            var bytes = _domain.PeekBytes(address, address + DataSize, false);
            CorruptCore_Extensions.AddValueToByteArrayUnchecked(ref bytes, 1, _domain.BigEndian);
            _domain.PokeBytes(address, bytes, _domain.BigEndian);
        }

        private void DecrementAddress(long address)
        {
            var bytes = _domain.PeekBytes(address, address + DataSize, false);
            CorruptCore_Extensions.AddValueToByteArrayUnchecked(ref bytes, -1, _domain.BigEndian);
            _domain.PokeBytes(address, bytes, _domain.BigEndian);
        }

        private string ValueString(long address)
        {
            if (address != -1)
            {
                var bytes = _domain.PeekBytes(address, address + DataSize, _domain.BigEndian);
                return CorruptCore_Extensions.BytesToHexString(bytes);
            }

            return "";
        }

        private string GetFindValues()
        {
            if (HighlightedAddress.HasValue)
            {
                var values = ValueString(HighlightedAddress.Value);
                return _secondaryHighlightedAddresses.Aggregate(values, (current, x) => current + ValueString(x));
            }

            return "";
        }

        private void HighlightSecondaries(string value, long found)
        {
            // This function assumes that the primary highlighted value has been set and sets the remaining characters in this string
            _secondaryHighlightedAddresses.Clear();

            var addrLength = DataSize * 2;
            if (value.Length <= addrLength)
            {
                return;
            }

            var numToHighlight = (value.Length / addrLength) - 1;

            for (var i = 0; i < numToHighlight; i += DataSize)
            {
                _secondaryHighlightedAddresses.Add(found + DataSize + i);
            }
        }

        private bool LoadTable(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            var file = new FileInfo(path);
            if (!file.Exists)
            {
                return false;
            }

            using (var sr = file.OpenText())
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    var parts = line.Split('=');
                    _textTable.Add(
                        int.Parse(parts[0],
                        NumberStyles.HexNumber), parts[1].First());
                }
            }

            return true;
        }

        #region Events

        #region File Menu

        private void CloseTableFileMenuItem_Click(object sender, EventArgs e)
        {
            _textTable.Clear();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Edit

        private void EditMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            var data = Clipboard.GetDataObject();
            PasteMenuItem.Enabled =
                (HighlightedAddress.HasValue || _secondaryHighlightedAddresses.Any()) &&
                data != null &&
                data.GetDataPresent(DataFormats.Text);

            FindNextMenuItem.Enabled = !string.IsNullOrWhiteSpace(_findStr);
        }

        private string MakeCopyExportString(bool export)
        {
            //make room for an array with _secondaryHighlightedAddresses and optionally HighlightedAddress
            long[] addresses = new long[_secondaryHighlightedAddresses.Count + (HighlightedAddress.HasValue ? 1 : 0)];

            //if there was actually nothing to do, return
            if (addresses.Length == 0)
            {
                return null;
            }

            //fill the array with _secondaryHighlightedAddresses
            for (int i = 0; i < _secondaryHighlightedAddresses.Count; i++)
            {
                addresses[i] = _secondaryHighlightedAddresses[i];
            }
            //and add HighlightedAddress if present
            if (HighlightedAddress.HasValue)
            {
                addresses[addresses.Length - 1] = HighlightedAddress.Value;
            }

            //these need to be sorted. it's not just for HighlightedAddress, _secondaryHighlightedAddresses can even be jumbled
            Array.Sort(addresses);

            //find the maximum length of the exported string
            int maximumLength = addresses.Length * (export ? 3 : 2) + 8;
            StringBuilder sb = new StringBuilder(maximumLength);

            //generate it differently for export (as you see it) or copy (raw bytes)
            if (export)
            {
                for (int i = 0; i < addresses.Length; i++)
                {
                    sb.Append(ValueString(addresses[i]));
                    if (i != addresses.Length - 1)
                    {
                        sb.Append(' ');
                    }
                }
            }
            else
            {
                for (int i = 0; i < addresses.Length; i++)
                {
                    long start = addresses[i];
                    long end = addresses[i] + DataSize - 1;
                    for (long a = start; a <= end; a++)
                    {
                        MakeValue(1, a, out int val);
                        sb.AppendFormat("{0:X2}", val);
                    }
                }
            }

            return sb.ToString();
        }

        private void ExportMenuItem_Click(object sender, EventArgs e)
        {
            var value = MakeCopyExportString(true);
            if (!string.IsNullOrEmpty(value))
            {
                Clipboard.SetDataObject(value);
            }
        }

        private void CopyMenuItem_Click(object sender, EventArgs e)
        {
            var value = MakeCopyExportString(false);
            if (!string.IsNullOrEmpty(value))
            {
                Clipboard.SetDataObject(value);
            }
        }

        private void PasteMenuItem_Click(object sender, EventArgs e)
        {
            var data = Clipboard.GetDataObject();

            if (data != null && !data.GetDataPresent(DataFormats.Text))
            {
                return;
            }

            var clipboardRaw = (string)data.GetData(DataFormats.Text);
            var hex = clipboardRaw.OnlyHex();

            var numBytes = hex.Length / 2;
            for (var i = 0; i < numBytes; i++)
            {
                var value = int.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber);
                var address = _addressHighlighted + i;

                if (address < _domain.Size)
                {
                    _domain.PokeByte(address, (byte)value);
                }
            }

            UpdateValues();
        }

        private bool _lastSearchWasText = false;

        private void SearchTypeChanged(bool isText)
        {
            _lastSearchWasText = isText;
        }

        private void FindMenuItem_Click(object sender, EventArgs e)
        {
            _findStr = GetFindValues();
            if (!_hexFind.IsHandleCreated || _hexFind.IsDisposed)
            {
                _hexFind = new HexFind
                {
                    InitialLocation = PointToScreen(AddressesLabel.Location),
                    InitialValue = _findStr,
                    SearchTypeChangedCallback = SearchTypeChanged,
                    InitialText = _lastSearchWasText
                };

                _hexFind.Show();
            }
            else
            {
                _hexFind.InitialValue = _findStr;
                _hexFind.Focus();
            }
        }

        private void FindNextMenuItem_Click(object sender, EventArgs e)
        {
            FindNext(_findStr, false);
        }

        private void FindPrevMenuItem_Click(object sender, EventArgs e)
        {
            FindPrev(_findStr, false);
        }

        #endregion

        #region Options

        private void OptionsSubMenu_DropDownOpened(object sender, EventArgs e)
        {
            BigEndianMenuItem.Checked = BigEndian;
            PokeAddressMenuItem.Enabled = true;
        }

        private void dataSizeToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            DataSizeByteMenuItem.Checked = DataSize == 1;
            DataSizeWordMenuItem.Checked = DataSize == 2;
            DataSizeDWordMenuItem.Checked = DataSize == 4;
        }

        private ToolStripItem GetMenuItem(MemoryInterface domain, Action<string> setCallback, string selected = "", int? maxSize = null)
        {
            var name = domain.Name;
            var item = new ToolStripMenuItem
            {
                Text = name,
                Enabled = !(maxSize.HasValue && domain.Size > maxSize.Value),
                Checked = name == selected
            };
            item.Click += (o, ev) => setCallback(name);
            return item;
        }

        private void MemoryDomainsMenuItem_MouseDown(object sender, MouseEventArgs e)
        {
            if (MemoryDomainsMenuItem.DropDownItems.Count == 0)
            {
                MemoryDomainsMenuItem.DropDownItems.Add(GetMenuItem(new NullMemoryInterface(), SetMemoryDomain, _domain.Name));
                MemoryDomainsMenuItem.ShowDropDown();
            }
            else
            {
                MemoryDomainsMenuItem.DropDownItems.Clear();
                foreach (var k in AllDomains.Values)
                {
                    MemoryDomainsMenuItem.DropDownItems.Add(GetMenuItem(k, SetMemoryDomain, _domain.Name));
                }
            }
        }

        private void MemoryDomainsMenuItem_DropDown(object sender, EventArgs e)
        {
        }

        private void DataSizeByteMenuItem_Click(object sender, EventArgs e)
        {
            SetDataSize(1);
        }

        private void DataSizeWordMenuItem_Click(object sender, EventArgs e)
        {
            SetDataSize(2);
        }

        private void DataSizeDWordMenuItem_Click(object sender, EventArgs e)
        {
            SetDataSize(4);
        }

        private void BigEndianMenuItem_Click(object sender, EventArgs e)
        {
            //BigEndian ^= true;
            //UpdateValues();
        }

        private void SwapBytesMenuItem_Click(object sender, EventArgs e)
        {
            SwapBytes ^= true;
            UpdateValues();
        }

        private void GoToAddressMenuItem_Click(object sender, EventArgs e)
        {
            var inputPrompt = new InputPrompt
            {
                Text = "Go to Address",
                StartLocation = this.PointToScreen(new Point(MemoryViewerBox.Location.X, MemoryViewerBox.Location.Y)),
                Message = "Enter a hexadecimal value"
            };

            var result = inputPrompt.ShowDialog();

            if (result == DialogResult.OK && inputPrompt.PromptText.IsHex())
            {
                GoToAddress(long.Parse(inputPrompt.PromptText, NumberStyles.HexNumber));
            }

            AddressLabel.Text = GenerateAddressString();
        }

        private void FreezeContextItem_Click(object sender, EventArgs e)
        {
            if (HighlightedAddress.HasValue)
            {
                if (IsFrozen(HighlightedAddress.Value))
                {
                    UnFreezeAddress(HighlightedAddress.Value);
                    UnfreezeSecondaries();
                }
                else
                {
                    FreezeAddress(HighlightedAddress.Value);
                    FreezeSecondaries();
                }
            }

            MemoryViewerBox.Refresh();
        }

        private void UnfreezeAllMenuItem_Click(object sender, EventArgs e)
        {
            StepActions.ClearStepBlastUnits();
        }

        private void PokeAddressMenuItem_Click(object sender, EventArgs e)
        {/*
            var addresses = new List<long>();
            if (HighlightedAddress.HasValue)
            {
                addresses.Add(HighlightedAddress.Value);
            }

            if (_secondaryHighlightedAddresses.Any())
            {
                addresses.AddRange(_secondaryHighlightedAddresses);
            }

            if (addresses.Any())
            {
                BlastUnit bu = new BlastUnit(StoreType.ONCE, StoreTime.IMMEDIATE, _domain.Name, address, _domain.Name, address, DataSize, _domain.BigEndian, 0, 0);;
                var poke = new RamPoke
                {
                    InitialLocation = this.ChildPointToScreen(AddressLabel),
                    ParentTool = this
                };

                var watches = addresses.Select(
                    address => Watch.GenerateWatch(
                        _domain,
                        address,
                        (WatchSize)DataSize,
                        Client.Common.DisplayType.Hex,
                        BigEndian));

                poke.SetWatch(watches);
                poke.ShowHawkDialog();
                UpdateValues();
            }*/
        }

        #endregion

        #region Settings Menu

        private void ResetColorsToDefaultMenuItem_Click(object sender, EventArgs e)
        {
            MemoryViewerBox.BackColor = Color.FromName("Control");
            MemoryViewerBox.ForeColor = Color.FromName("ControlText");
            this.HexMenuStrip.BackColor = Color.FromName("Control");
            Header.BackColor = Color.FromName("Control");
            Header.ForeColor = Color.FromName("ControlText");
        }

        #endregion

        #region Context Menu and Dialog Events

        private void HexEditor_Resize(object sender, EventArgs e)
        {
            SetUpScrollBar();
            UpdateValues();
        }

        private void HexEditor_ResizeEnd(object sender, EventArgs e)
        {
            SetUpScrollBar();
        }

        private void HexEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.G)
            {
                GoToAddressMenuItem_Click(sender, e);
                return;
            }

            if (e.Control && e.KeyCode == Keys.P)
            {
                PokeAddressMenuItem_Click(sender, e);
                return;
            }
            if (e.Control && e.KeyCode == Keys.F)
            {
                FindMenuItem_Click(sender, e);
                return;
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                CopyMenuItem_Click(sender, e);
                return;
            }
            if (e.Control && e.KeyCode == Keys.E)
            {
                ExportMenuItem_Click(sender, e);
                return;
            }
            if (e.Control && e.KeyCode == Keys.V)
            {
                PasteMenuItem_Click(sender, e);
                return;
            }
            if (e.Shift && e.KeyCode == Keys.Delete)
            {
                UnfreezeAllMenuItem_Click(sender, e);
                return;
            }
            if (e.KeyCode == Keys.F2)
            {
                FindNextMenuItem_Click(sender, e);
                return;
            }
            if (e.KeyCode == Keys.F3)
            {
                FindPrevMenuItem_Click(sender, e);
                return;
            }


            long newHighlighted;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    newHighlighted = _addressHighlighted - 16;
                    if (e.Modifiers == Keys.Shift)
                    {
                        for (var i = newHighlighted + DataSize; i <= _addressHighlighted; i += DataSize)
                        {
                            AddToSecondaryHighlights(i);
                        }

                        GoToAddress(newHighlighted);
                    }
                    else
                    {
                        _secondaryHighlightedAddresses.Clear();
                        GoToAddress(newHighlighted);
                    }

                    break;
                case Keys.Down:
                    newHighlighted = _addressHighlighted + 16;
                    if (e.Modifiers == Keys.Shift)
                    {
                        for (var i = _addressHighlighted; i < newHighlighted; i += DataSize)
                        {
                            AddToSecondaryHighlights(i);
                        }

                        GoToAddress(newHighlighted);
                    }
                    else
                    {
                        _secondaryHighlightedAddresses.Clear();
                        GoToAddress(newHighlighted);
                    }

                    break;
                case Keys.Left:
                    newHighlighted = _addressHighlighted - (1 * DataSize);
                    if (e.Modifiers == Keys.Shift)
                    {
                        AddToSecondaryHighlights(_addressHighlighted);
                        GoToAddress(newHighlighted);
                    }
                    else
                    {
                        _secondaryHighlightedAddresses.Clear();
                        GoToAddress(newHighlighted);
                    }

                    break;
                case Keys.Right:
                    newHighlighted = _addressHighlighted + (1 * DataSize);
                    if (e.Modifiers == Keys.Shift)
                    {
                        AddToSecondaryHighlights(_addressHighlighted);
                        GoToAddress(newHighlighted);
                    }
                    else
                    {
                        _secondaryHighlightedAddresses.Clear();
                        GoToAddress(newHighlighted);
                    }

                    break;
                case Keys.PageUp:
                    newHighlighted = _addressHighlighted - (_rowsVisible * 16);
                    if (e.Modifiers == Keys.Shift)
                    {
                        for (var i = newHighlighted + 1; i <= _addressHighlighted; i += DataSize)
                        {
                            AddToSecondaryHighlights(i);
                        }

                        GoToAddress(newHighlighted);
                    }
                    else
                    {
                        _secondaryHighlightedAddresses.Clear();
                        GoToAddress(newHighlighted);
                    }

                    break;
                case Keys.PageDown:
                    newHighlighted = _addressHighlighted + (_rowsVisible * 16);
                    if (e.Modifiers == Keys.Shift)
                    {
                        for (var i = _addressHighlighted + 1; i < newHighlighted; i += DataSize)
                        {
                            AddToSecondaryHighlights(i);
                        }

                        GoToAddress(newHighlighted);
                    }
                    else
                    {
                        _secondaryHighlightedAddresses.Clear();
                        GoToAddress(newHighlighted);
                    }

                    break;
                case Keys.Tab:
                    _secondaryHighlightedAddresses.Clear();
                    if (e.Modifiers == Keys.Shift)
                    {
                        GoToAddress(_addressHighlighted - 8);
                    }
                    else
                    {
                        GoToAddress(_addressHighlighted + 8);
                    }

                    break;
                case Keys.Home:
                    if (e.Modifiers == Keys.Shift)
                    {
                        for (var i = 1; i <= _addressHighlighted; i += DataSize)
                        {
                            AddToSecondaryHighlights(i);
                        }

                        GoToAddress(0);
                    }
                    else
                    {
                        _secondaryHighlightedAddresses.Clear();
                        GoToAddress(0);
                    }

                    break;
                case Keys.End:
                    newHighlighted = _domain.Size - DataSize;
                    if (e.Modifiers == Keys.Shift)
                    {
                        for (var i = _addressHighlighted; i < newHighlighted; i += DataSize)
                        {
                            AddToSecondaryHighlights(i);
                        }

                        GoToAddress(newHighlighted);
                    }
                    else
                    {
                        _secondaryHighlightedAddresses.Clear();
                        GoToAddress(newHighlighted);
                    }

                    break;
                case Keys.Add:
                    IncrementContextItem_Click(sender, e);
                    break;
                case Keys.Subtract:
                    DecrementContextItem_Click(sender, e);
                    break;
                case Keys.Space:
                    break;
                case Keys.Delete:
                    break;
                case Keys.Escape:
                    _secondaryHighlightedAddresses.Clear();
                    ClearHighlighted();
                    break;
            }
        }

        private void HexEditor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!IsHexKeyCode(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            if ((ModifierKeys & (Keys.Control | Keys.Shift | Keys.Alt)) != 0)
            {
                return;
            }

            switch (DataSize)
            {
                default:
                case 1:
                    if (_nibbles[0] == 'G')
                    {
                        _nibbles[0] = ForceCorrectKeyString(e.KeyChar);
                    }
                    else
                    {
                        var temp = _nibbles[0].ToString() + ForceCorrectKeyString(e.KeyChar);
                        var x = byte.Parse(temp, NumberStyles.HexNumber);
                        _domain.PokeByte(_addressHighlighted, x);
                        ClearNibbles();
                        SetHighlighted(_addressHighlighted + 1);
                        UpdateValues();
                        Refresh();
                    }

                    break;
                case 2:
                    if (_nibbles[0] == 'G')
                    {
                        _nibbles[0] = ForceCorrectKeyString(e.KeyChar);
                    }
                    else if (_nibbles[1] == 'G')
                    {
                        _nibbles[1] = ForceCorrectKeyString(e.KeyChar);
                    }
                    else if (_nibbles[2] == 'G')
                    {
                        _nibbles[2] = ForceCorrectKeyString(e.KeyChar);
                    }
                    else if (_nibbles[3] == 'G')
                    {
                        var temp = _nibbles[0].ToString() + _nibbles[1];
                        var x1 = byte.Parse(temp, NumberStyles.HexNumber);

                        var temp2 = _nibbles[2].ToString() + e.KeyChar;
                        var x2 = byte.Parse(temp2, NumberStyles.HexNumber);

                        PokeWord(_addressHighlighted, x1, x2);
                        ClearNibbles();
                        SetHighlighted(_addressHighlighted + 2);
                        UpdateValues();
                        Refresh();
                    }

                    break;
                case 4:
                    if (_nibbles[0] == 'G')
                    {
                        _nibbles[0] = ForceCorrectKeyString(e.KeyChar);
                    }
                    else if (_nibbles[1] == 'G')
                    {
                        _nibbles[1] = ForceCorrectKeyString(e.KeyChar);
                    }
                    else if (_nibbles[2] == 'G')
                    {
                        _nibbles[2] = ForceCorrectKeyString(e.KeyChar);
                    }
                    else if (_nibbles[3] == 'G')
                    {
                        _nibbles[3] = ForceCorrectKeyString(e.KeyChar);
                    }
                    else if (_nibbles[4] == 'G')
                    {
                        _nibbles[4] = ForceCorrectKeyString(e.KeyChar);
                    }
                    else if (_nibbles[5] == 'G')
                    {
                        _nibbles[5] = ForceCorrectKeyString(e.KeyChar);
                    }
                    else if (_nibbles[6] == 'G')
                    {
                        _nibbles[6] = ForceCorrectKeyString(e.KeyChar);
                    }
                    else if (_nibbles[7] == 'G')
                    {
                        var temp = _nibbles[0].ToString() + _nibbles[1];
                        var x1 = byte.Parse(temp, NumberStyles.HexNumber);

                        var temp2 = _nibbles[2].ToString() + _nibbles[3];
                        var x2 = byte.Parse(temp2, NumberStyles.HexNumber);

                        var temp3 = _nibbles[4].ToString() + _nibbles[5];
                        var x3 = byte.Parse(temp3, NumberStyles.HexNumber);

                        var temp4 = _nibbles[6].ToString() + ForceCorrectKeyString(e.KeyChar);
                        var x4 = byte.Parse(temp4, NumberStyles.HexNumber);

                        PokeWord(_addressHighlighted, x1, x2);
                        PokeWord(_addressHighlighted + 2, x3, x4);
                        ClearNibbles();
                        SetHighlighted(_addressHighlighted + 4);
                        UpdateValues();
                        Refresh();
                    }

                    break;
            }

            UpdateValues();
        }

        private void ViewerContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            var data = Clipboard.GetDataObject();

            FreezeContextItem.Visible = false;

            CopyContextItem.Visible =
                HighlightedAddress.HasValue || _secondaryHighlightedAddresses.Any();
            FreezeContextItem.Visible =
                IncrementContextItem.Visible =
                    DecrementContextItem.Visible =
                        ContextSeparator2.Visible =
                            (HighlightedAddress.HasValue || _secondaryHighlightedAddresses.Any());

            PasteContextItem.Visible = data != null && data.GetDataPresent(DataFormats.Text);

            if (HighlightedAddress.HasValue && IsFrozen(HighlightedAddress.Value))
            {
                FreezeContextItem.Text = "Un&freeze";
            }
            else
            {
                FreezeContextItem.Text = "&Freeze";
            }
        }

        private void IncrementContextItem_Click(object sender, EventArgs e)
        {
            if (HighlightedAddress.HasValue)
            {
                IncrementAddress(HighlightedAddress.Value);
            }

            _secondaryHighlightedAddresses.ForEach(IncrementAddress);

            UpdateValues();
        }

        private void DecrementContextItem_Click(object sender, EventArgs e)
        {
            if (HighlightedAddress.HasValue)
            {
                DecrementAddress(HighlightedAddress.Value);
            }

            _secondaryHighlightedAddresses.ForEach(DecrementAddress);

            UpdateValues();
        }

        #endregion

        #region MemoryViewer Events

        private void HexEditor_MouseWheel(object sender, MouseEventArgs e)
        {
            var delta = 0;
            if (e.Delta > 0)
            {
                delta = -1;
            }
            else if (e.Delta < 0)
            {
                delta = 1;
            }

            var newValue = HexScrollBar.Value + delta;
            if (newValue < HexScrollBar.Minimum)
            {
                newValue = HexScrollBar.Minimum;
            }

            if (newValue > HexScrollBar.Maximum - HexScrollBar.LargeChange + 1)
            {
                newValue = HexScrollBar.Maximum - HexScrollBar.LargeChange + 1;
            }

            if (newValue != HexScrollBar.Value)
            {
                HexScrollBar.Value = newValue;
                MemoryViewerBox.Refresh();
            }
        }

        private void MemoryViewerBox_Paint(object sender, PaintEventArgs e)
        {
            var infiniteUnits = StepActions.GetAppliedInfiniteUnits();
            foreach (var bu in infiniteUnits.Layer)
            {
                if (IsVisible(bu.Address))
                {
                    if (_domain.ToString() == bu.Domain)
                    {
                        var gaps = bu.Precision - DataSize;

                        if (bu.Precision == 4 && DataSize == 2)
                        {
                            gaps -= 1;
                        }

                        if (gaps < 0) { gaps = 0; }

                        var width = (fontWidth * 2 * bu.Precision) + (gaps * fontWidth);

                        var rect = new Rectangle(GetAddressCoordinates(bu.Address), new Size(width, fontHeight));
                        e.Graphics.DrawRectangle(new Pen(Brushes.Black), rect);
                        e.Graphics.FillRectangle(new SolidBrush(Color.Cyan), rect);
                    }
                }
            }

            if (_addressHighlighted >= 0 && IsVisible(_addressHighlighted))
            {
                // Create a slight offset to increase rectangle sizes
                var point = GetAddressCoordinates(_addressHighlighted);
                var textX = (int)GetTextX(_addressHighlighted);
                var textpoint = new Point(textX, point.Y);

                var rect = new Rectangle(point, new Size(fontWidth * 2 * DataSize + (NeedsExtra(_addressHighlighted) ? fontWidth : 0) + 3, fontHeight));
                e.Graphics.DrawRectangle(new Pen(Brushes.Black), rect);

                var textrect = new Rectangle(textpoint, new Size(fontWidth * DataSize, fontHeight));

                if (CorruptCore.StepActions.InfiniteUnitExists(_domain.Name, _addressHighlighted))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.Cyan), rect);
                    e.Graphics.FillRectangle(new SolidBrush(Color.Cyan), textrect);
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.LightPink), rect);
                    e.Graphics.FillRectangle(new SolidBrush(Color.LightPink), textrect);
                }
            }

            foreach (var address in _secondaryHighlightedAddresses)
            {
                if (IsVisible(address))
                {
                    var point = GetAddressCoordinates(address);
                    var textX = (int)GetTextX(address);
                    var textpoint = new Point(textX, point.Y);

                    var rect = new Rectangle(point, new Size(fontWidth * 2 * DataSize + 3, fontHeight));
                    e.Graphics.DrawRectangle(new Pen(Brushes.Black), rect);

                    var textrect = new Rectangle(textpoint, new Size(fontWidth * DataSize, fontHeight));

                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0x44, Color.LightPink)), rect);
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0x44, Color.LightPink)), textrect);
                }
            }

            if (HasNibbles())
            {
                //e.Graphics.DrawString(MakeNibbles(), new Font("Courier New", 8, FontStyle.Italic), Brushes.Black, new Point(158, 4));
            }
        }

        private void AddressesLabel_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseIsDown = false;
        }

        private void AddressesLabel_MouseMove(object sender, MouseEventArgs e)
        {
            _addressOver = GetPointedAddress(e.X, e.Y);

            if (_mouseIsDown)
            {
                DoShiftClick();
                UpdateFormText();
                MemoryViewerBox.Refresh();
            }
        }

        private void AddressesLabel_MouseLeave(object sender, EventArgs e)
        {
            _addressOver = -1;
            MemoryViewerBox.Refresh();
        }

        private void AddressesLabel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var pointedAddress = GetPointedAddress(e.X, e.Y);
                if (pointedAddress >= 0)
                {
                    if ((ModifierKeys & Keys.Control) == Keys.Control)
                    {
                        if (pointedAddress == _addressHighlighted)
                        {
                            ClearHighlighted();
                        }
                        else if (_secondaryHighlightedAddresses.Contains(pointedAddress))
                        {
                            _secondaryHighlightedAddresses.Remove(pointedAddress);
                        }
                        else
                        {
                            _secondaryHighlightedAddresses.Add(pointedAddress);
                        }
                    }
                    else if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                    {
                        DoShiftClick();
                    }
                    else
                    {
                        _secondaryHighlightedAddresses.Clear();
                        _findStr = "";
                        SetHighlighted(pointedAddress);
                    }

                    MemoryViewerBox.Refresh();
                }

                _mouseIsDown = true;
            }
        }

        private bool _programmaticallyChangingValue = false;

        private void HexScrollBar_ValueChanged(object sender, EventArgs e)
        {
            if (!_programmaticallyChangingValue)
            {
                if (HexScrollBar.Value < 0)
                {
                    _programmaticallyChangingValue = true;
                    HexScrollBar.Value = 0;
                    _programmaticallyChangingValue = false;
                }

                UpdateValues();
            }
        }

        #endregion

        private void HexMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        #endregion

        private void UnFreezeAddress(long address)
        {
            StepActions.TryRemoveInfiniteStepUnits(_domain.Name, address);
        }

        private void UnfreezeSecondaries()
        {
            foreach (var x in _secondaryHighlightedAddresses)
            {
                UnFreezeAddress(x);
            }
        }

        private void FreezeAddress(long address)
        {
            BlastUnit bu = new BlastUnit(StoreType.ONCE, StoreTime.IMMEDIATE, _domain.Name, address, _domain.Name, address, DataSize, _domain.BigEndian, 0, 0);
            bu.Apply(false);
        }

        private void FreezeSecondaries()
        {
            foreach (var x in _secondaryHighlightedAddresses)
            {
                FreezeAddress(x);
            }
        }

        private bool IsFrozen(long address)
        {
            return StepActions.InfiniteUnitExists(_domain.Name, address);
        }

        private void OptionsSubMenu_Click(object sender, EventArgs e)
        {
        }
    }
}
