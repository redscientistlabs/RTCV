namespace RTCV.UI
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using RTCV.Common;

    public interface ISubForm
    {
        //Interface used for added contrals in SubForms

        bool SubForm_HasLeftButton { get; }
        bool SubForm_HasRightButton { get; }
        string SubForm_LeftButtonText { get; }
        string SubForm_RightButtonText { get; }
        void SubForm_LeftButton_Click();
        void SubForm_RightButton_Click();
        void OnShown(); //The included OnShown sucks
        void OnHidden(); //There's no OnHidden and VisibleChanged sucks
    }

    public interface ITileForm
    {
        //Interface used for added contrals in TileForms

        int TilesX { get; set; }
        int TilesY { get; set; }
        bool CanPopout { get; set; }
    }

    public interface IBlockable
    {
        //Interface used for querying blockable fornms/panels and do actions of them
        Panel blockPanel { get; set; }
        //void BlockView();
        //void UnblockView();
    }

    public class CanvasGrid
    {
        //grid that represents the layout of a single form

        public int x = 0;
        public int y = 0;
        public Form[,] gridComponent;
        public Size?[,] gridComponentSize;
        public bool?[,] gridComponentDisplayHeader;

        public string GridName = "";
        internal bool isResizable = false;

        public CanvasGrid(int _x, int _y, string _GridName)
        {
            x = _x;
            y = _y;
            gridComponent = new Form[x, y];
            gridComponentSize = new Size?[x, y];
            gridComponentDisplayHeader = new bool?[x, y];
            GridName = _GridName;
        }

        public void SetTileForm(Form componentForm, int tilePosX, int tilePosY, int tileSizeX, int tileSizeY, bool displayHeader, AnchorStyles anchor = (AnchorStyles.Top | AnchorStyles.Left))
        {
            //removes tileForm position if already exists
            for (int _x = 0; _x < x; _x++)
            {
                for (int _y = 0; _y < y; _y++)
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
            if (tilePosX < x && tilePosY < y)
            {
                gridComponent[tilePosX, tilePosY] = componentForm;
                gridComponentSize[tilePosX, tilePosY] = new Size(tileSizeX, tileSizeY);
                gridComponentDisplayHeader[tilePosX, tilePosY] = displayHeader;
            }

            componentForm.Anchor = anchor;
        }

        internal void LoadToMain()
        {
            UI_CanvasForm.loadTileFormMain(this);
        }

        internal void LoadToNewWindow(string GridID = null, bool silent = false)
        {
            UI_CanvasForm.loadTileFormExtraWindow(this, GridID, silent);
        }

        internal static void LoadCustomLayout()
        {
            string customLayoutPath = Path.Combine(RTCV.CorruptCore.RtcCore.RtcDir, "CustomLayout.txt");
            string[] allLines = File.ReadAllLines(customLayoutPath);

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
                                tileForm = (Form)S.GET(Type.GetType("RTCV.UI." + formName));
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
