namespace RTCV.UI
{
    using System.Windows.Forms;

    public interface IBlockable
    {
        //Interface used for querying blockable fornms/panels and do actions of them
        Panel blockPanel { get; set; }
        //void BlockView();
        //void UnblockView();
    }
}
