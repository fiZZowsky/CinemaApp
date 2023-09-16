namespace TicketScanner;

public partial class Scanner : ContentPage
{
    public Scanner()
    {
        InitializeComponent();

        cameraView.BarCodeOptions = new()
        {
            PossibleFormats = { ZXing.BarcodeFormat.QR_CODE },
            TryHarder = true,
            AutoRotate = true,
            TryInverted = true
        };
    }

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.Cameras.Count > 0)
        {
            cameraView.Camera = cameraView.Cameras.First();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await cameraView.StopCameraAsync();
                await cameraView.StartCameraAsync();
            });

        }
    }

    private void cameraView_BarcodeDetected(object sender,
        Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            string barcodeResult = args.Result[0].Text;
            DisplayAlert("Ticket", barcodeResult, "OK");
        });
    }
}