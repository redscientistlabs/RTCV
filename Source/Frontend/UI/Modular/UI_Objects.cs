namespace RTCV.UI
{
    using System.Windows.Forms;

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
}
