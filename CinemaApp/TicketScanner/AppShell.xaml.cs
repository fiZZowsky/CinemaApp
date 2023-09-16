namespace TicketScanner;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("Scanner", typeof(Scanner));
    }
}
