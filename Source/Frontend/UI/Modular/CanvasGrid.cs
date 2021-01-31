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

        public int X { get; private set; } = 0;
        public int Y { get; private set; } = 0;
        public Form[,] gridComponent { get; private set; }
        public Size?[,] gridComponentSize { get; private set; }
        public bool?[,] gridComponentDisplayHeader { get; private set; }

        public string GridName { get; private set; } = "";
        internal bool isResizable = false;

        public CanvasGrid(int x, int y, string gridName)
        {
            X = x;
            Y = y;
            gridComponent = new Form[X, Y];
            gridComponentSize = new Size?[X, Y];
            gridComponentDisplayHeader = new bool?[X, Y];
            GridName = gridName;
        }

        internal void SetTileForm(Form componentForm, int tilePosX, int tilePosY, int tileSizeX, int tileSizeY, bool displayHeader, AnchorStyles anchor = (AnchorStyles.Top | AnchorStyles.Left))
        {
            //removes tileForm position if already exists
            for (int _x = 0; _x < X; _x++)
            {
                for (int _y = 0; _y < Y; _y++)
                {
                    if (gridComponent[_x, _y] == componentForm)
                    {
                        gridComponent[_x, _y] = null;
                        gridComponentSize[_x, _y] = null;
                        gridComponentDisplayHeader[_x, _y] = null;
                    }
                }
            }

            //place tileForm if within grid space
            if (tilePosX < X && tilePosY < Y)
            {
                gridComponent[tilePosX, tilePosY] = componentForm;
                gridComponentSize[tilePosX, tilePosY] = new Size(tileSizeX, tileSizeY);
                gridComponentDisplayHeader[tilePosX, tilePosY] = displayHeader;
            }

            componentForm.Anchor = anchor;
        }

        internal void LoadToMain()
        {
            CanvasForm.loadTileFormMain(this);
        }

        internal void LoadToNewWindow(string GridID = null, bool silent = false)
        {
            CanvasForm.loadTileFormExtraWindow(this, GridID, silent);
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
                    case "CreateGrid":
                        {
                            cuGrid = new CanvasGrid(gridSizeX, gridSizeY, gridName);
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

                            Form tileForm = null;

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
                                        continue;
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
                            if (data == "Main")
                            {
                                cuGrid.LoadToMain();
                            }
                            else
                            {
                                cuGrid.LoadToNewWindow("External");
                            }

                            break;
                        }
                }
            }
        }
    }
}
