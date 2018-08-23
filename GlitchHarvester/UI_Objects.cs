using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RTCV.UI
{

    public interface ISubForm
    {
        //Interface used for added contrals in SubForms

        bool HasCancelButton { get; set; }
        void SubForm_Cancel();
        void SubForm_Ok();
    }


    public interface ITileForm
    {
        //Interface used for added contrals in TileForms

        int TilesX { get; set; }
        int TilesY { get; set; }
        bool CanPopout { get; set; }

    }

    public class MultiGrid
    {
        // multiple grids that will load on multiple forms

        public CanvasGrid[] grids;

        public MultiGrid(params CanvasGrid[] _grids)
        {
            grids = _grids;
        }

        public void Load()
        {
            //Deploys and loads all the grids in the multigrid object

            for(int i=0;i<grids.Count();i++)
            {
                if (i == 0)
                    UI_CanvasForm.loadTileFormMain(grids[i]);
                else
                    UI_CanvasForm.loadTileFormExtraWindow(grids[i]);

            }
        }
    }

    public class CanvasGrid
    {
        //grid that represents the layout of a single form

        public int x = 0;
        public int y = 0;
        public string[,] gridComponent;
        public Size?[,] gridComponentSize;
        public bool?[,] gridComponentDisplayHeader;

        public string GridName;

        public CanvasGrid(int _x, int _y, string _GridName)
        {
            x = _x;
            y = _y;
            gridComponent = new string[x,y];
            gridComponentSize = new Size?[x, y];
            gridComponentDisplayHeader = new bool?[x, y];
            GridName = _GridName;
        }

        public void SetTileForm(string componentFormName, int tilePosX, int tilePosY, int tileSizeX, int tileSizeY, bool displayHeader = true)
        {
            //removes tileForm position if already exists
            for (int _x = 0; _x < x; _x++)
                for (int _y = 0; _y < y; _y++)
                    if (gridComponent[_x, _y] == componentFormName)
                    {
                        gridComponent[_x, _y] = null;
                        gridComponentSize[_x, _y] = null;
                        gridComponentDisplayHeader[_x, _y] = null;
                    }

            //place tileForm if within grid space
            if (tilePosX < x && tilePosY < y)
            {
                gridComponent[tilePosX, tilePosY] = componentFormName;
                gridComponentSize[tilePosX, tilePosY] = new Size(tileSizeX, tileSizeY);
                gridComponentDisplayHeader[tilePosX, tilePosY] = displayHeader;
            }

        }

        internal void LoadToMain()
        {
            UI_CanvasForm.loadTileFormMain(this);
        }

        internal void LoadToNewWindow()
        {
            UI_CanvasForm.loadTileFormExtraWindow(this);
        }
    }

}
