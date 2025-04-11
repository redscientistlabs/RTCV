using RTCV.NetCore;
using RTCV.UI.Modular;

namespace RTCV.UI
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using RTCV.Common;
    using RTCV.CorruptCore;
    using SlimDX;

    public class CanvasGrid
    {
        //grid that represents the layout of a single form

        public int Width { get; private set; } = 0;
        public int Height { get; private set; } = 0;
        public int MinimumWidth { get; private set; } = 0;
        public int MinimumHeight { get; private set; } = 0;
        public Form[,] gridComponent { get; private set; }
        public Size?[,] gridComponentSize { get; private set; }
        public bool?[,] gridComponentDisplayHeader { get; private set; }
        public AnchorStyles?[,] gridComponentAnchor { get; private set; }

        public string GridName { get; private set; } = "";
        internal bool isResizable = false;

        public CanvasGrid(int width, int height, string gridName) : this(width, height, width, height, gridName)
        {
        }

        public CanvasGrid(int width, int height, int minWidth, int minHeight, string gridName)
        {
            this.Width = width;
            this.Height = height;
            this.MinimumWidth = minWidth;
            this.MinimumHeight = minHeight;
            this.gridComponent = new Form[Width, Height];
            this.gridComponentSize = new Size?[Width, Height];
            this.gridComponentDisplayHeader = new bool?[Width, Height];
            this.gridComponentAnchor = new AnchorStyles?[Width, Height];
            this.GridName = gridName;
        }

        internal void SetTileForm(Form componentForm, int tilePosX, int tilePosY, int tileSizeX, int tileSizeY, bool displayHeader, AnchorStyles anchor = (AnchorStyles.Top | AnchorStyles.Left))
        {
            //removes tileForm position if already exists
            for (int x = 0; x < this.Width; x++)
            {
                for (int y = 0; y < this.Height; y++)
                {
                    if (this.gridComponent[x, y] == componentForm)
                    {
                        this.gridComponent[x, y] = null;
                        this.gridComponentSize[x, y] = null;
                        this.gridComponentDisplayHeader[x, y] = null;
                        this.gridComponentAnchor[x, y] = null;
                    }
                }
            }

            //place tileForm if within grid space
            if (tilePosX < Width && tilePosY < Height)
            {
                gridComponent[tilePosX, tilePosY] = componentForm;
                gridComponentSize[tilePosX, tilePosY] = new Size(tileSizeX, tileSizeY);
                gridComponentDisplayHeader[tilePosX, tilePosY] = displayHeader;
                gridComponentAnchor[tilePosX, tilePosY] = anchor;
            }

            componentForm.Anchor = anchor;
        }

        internal void LoadToMain(bool dontAnchor = false, bool setDefault = true)
        {
            if (setDefault)
                S.GET<CoreForm>().SetDefaultGrid(this);

            if (Params.IsParamSet("GH_OPEN_MAIN"))
            {
                // if this actually is the GH grid, it's ok, it's set to true at the end of CoreForm.OpenGlitchHarvester()
                S.GET<CoreForm>().pnGlitchHarvesterOpen.Visible = false;
            }
            CanvasForm.loadTileFormMain(this, dontAnchor);
        }

        internal void LoadToNewWindow(string gridId = null, bool silent = false, bool dontAnchor = false)
        {
            CanvasForm.loadTileFormExtraWindow(this, gridId, silent, dontAnchor);
        }

        internal static FileInfo[] GetEnabledCustomLayouts()
        {
            string customLayoutsPath = Path.Combine(RtcCore.RtcDir, "LAYOUTS");
            return Directory.GetFiles(customLayoutsPath).Select(it => new FileInfo(it)).Where(it => !it.Name.StartsWith("_")).ToArray();
        }

        internal static void LoadCustomLayout(string targetLayout = null)
        {
            var legitLayouts = GetEnabledCustomLayouts().FirstOrDefault();
            if (legitLayouts == null)
                return;

            string[] allLines;

            if (targetLayout != null)
                allLines = File.ReadAllLines(targetLayout);
            else
                allLines = File.ReadAllLines(legitLayouts.FullName);

            int gridSizeX = 26;
            int gridSizeY = 19;
            int minSizeX = -1;
            int minSizeY = -1;
            string gridName = "Custom Grid";
            CanvasGrid cuGrid = new CanvasGrid(gridSizeX, gridSizeY, gridName);

            //foreach(string line in allLines.Select(it => it.Trim()))
            for (int i = 0; i < allLines.Length; i++)
            {
                string line = allLines[i].Trim();

                if (line.Length == 0 || line.StartsWith("//"))
                {
                    continue;
                }

                if (line.Contains("//"))
                {
                    string[] lineParts = line.Split('/');
                    line = lineParts[0].Trim();
                }

                string[] parts = line.Split(':');

                string command = parts[0];
                string data = (parts.Length > 1 ? parts[1] : "");

                switch (command)
                {
                    case "GridName":
                        {
                            gridName = data;
                            break;
                        }
                    case "GridSize":
                    {
                        string[] subData = data.Split(',');

                        gridSizeX = Convert.ToInt32(subData[0].Trim());
                        gridSizeY = Convert.ToInt32(subData[1].Trim());

                        break;
                    }
                    case "MinimumSize":
                    {
                        string[] subData = data.Split(',');

                        minSizeX = Convert.ToInt32(subData[0].Trim());
                        minSizeY = Convert.ToInt32(subData[1].Trim());

                        break;
                    }
                    case "CreateGrid":
                        {
                            if (minSizeX == -1 || minSizeY == -1)
                            {
                                cuGrid = new CanvasGrid(gridSizeX, gridSizeY, gridName);
                            }
                            else
                            {
                                cuGrid = new CanvasGrid(gridSizeX, gridSizeY, minSizeX, minSizeY, gridName);
                            }

                            break;
                        }
                    case "IsResizable":
                        {
                            cuGrid.isResizable = true;
                            break;
                        }
                    case "SetTileForm":
                        {
                            string[] subData = data.Split(',');

                            string formName = subData[0].Trim();

                            int formGridPosX = Convert.ToInt32(subData[1].Trim());
                            int formGridPosY = Convert.ToInt32(subData[2].Trim());
                            int formGridSizeX = Convert.ToInt32(subData[3].Trim());
                            int formGridSizeY = Convert.ToInt32(subData[4].Trim());

                            AnchorStyles formGridAnchor = (AnchorStyles.Top | AnchorStyles.Left);

                            if (subData.Length > 5 && !string.IsNullOrWhiteSpace(subData[5]))
                            {
                                formGridAnchor = (AnchorStyles)Convert.ToInt32(subData[5].Trim());
                            }

                            Form tileForm;

                            if (formName == "MemoryTools")
                            {
                                tileForm = UICore.mtForm;
                            }
                            else
                            {
                                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                                Type t = null;

                                foreach (var ass in assemblies)
                                {
                                    try
                                    {
                                        var types = ass.GetTypes();
                                        var type = types.FirstOrDefault(iterator => iterator.FullName.Contains(formName));
                                        if (type != null)
                                        {
                                            t = type;
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _ = ex;
                                    }
                                }

                                if (t != null)
                                {
                                    tileForm = (Form)S.GET(t);
                                }
                                else
                                {
                                    MessageBox.Show($"Could not find the Form type {formName} referenced in the Custom Layout.");
                                    return;
                                }
                            }

                            cuGrid.SetTileForm(tileForm, formGridPosX, formGridPosY, formGridSizeX, formGridSizeY, true, formGridAnchor);

                            break;
                        }
                    case "LoadTo":
                        {
                            var coreForm = S.GET<CoreForm>();
                            if (data == "Main")
                            {
                                cuGrid.LoadToMain();
                            }
                            else
                            {
                                coreForm.SetDefaultGrid(cuGrid);
                                cuGrid.LoadToNewWindow("External");
                            }

                            break;
                        }
                }
            }

            var form = CanvasForm.GetExtraForm("External");
            if (form is null)
            {
                return;
            }

            form.FormClosing -= RemoveExternalForm;
            form.FormClosing += RemoveExternalForm;
        }
        
        private static void RemoveExternalForm(object sender, FormClosingEventArgs e)
        {
            var coreForm = S.GET<CoreForm>();
            coreForm.PreviousGrids[1] = coreForm.PreviousGrids[(coreForm.ExternalIndex + 1) % 1];
            coreForm.ExternalIndex = -1;
            
            // if the external form had any modules that should also be in the main form's grid, they need to be put back
            coreForm.PreviousGrids[1].LoadToMain();
        }
    }
}
