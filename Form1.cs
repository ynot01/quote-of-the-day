namespace zen_quotes;
using System.Diagnostics;
using System.Text.Json;
using ZenApi;

public partial class Form1 : Form
{
    protected override CreateParams CreateParams
    {
        get
        {
            const int WS_EX_LAYERED = 0x80000;
            const int WS_EX_TRANSPARENT = 0x20;
            CreateParams crepa = base.CreateParams;
            crepa.ExStyle |= WS_EX_LAYERED;
            crepa.ExStyle |= WS_EX_TRANSPARENT;
            return crepa;
        }
    }

    static async void Loop(HttpClient client, Label quoteLabel, Label authorLabel) {
        Stopwatch timer = new();
        timer.Start();
        bool firstTime = true;
        while(timer.Elapsed.TotalSeconds > 30 || firstTime)
            {
                firstTime = false;
                await using Stream stream = await client.GetStreamAsync("https://zenquotes.io/api/today");
                var zen_response = await JsonSerializer.DeserializeAsync<List<ZenApi>>(stream);
                foreach (var response in zen_response ?? []) {
                    quoteLabel.Text = "\""+response.CurrentQuote+"\"";
                    authorLabel.Text = "-"+response.CurrentAuthor;
                };

                timer.Restart();
            }
        }

    public Form1()
    {
        InitializeComponent();
        StartPosition = FormStartPosition.Manual;
        // Location = new Point(0, 0);
        if (Screen.PrimaryScreen == null) {
            Console.WriteLine("No primary screen found.");
            Application.Exit();
            return;
        }
        AutoSize = true;
        Left = Screen.PrimaryScreen.WorkingArea.Width - Width;
        Top = Screen.PrimaryScreen.WorkingArea.Height - Height;
        TopMost = true;
        Opacity = 0.5;
        BackColor = Color.DimGray;
        TransparencyKey = Color.DimGray;
        FormBorderStyle = FormBorderStyle.None; // borderless
        Label quoteLabel = new()
        {
            Font = new("Arial", 16, FontStyle.Bold),
            ForeColor = Color.Transparent,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
            Text = "Loading quote..."
        };
        Label authorLabel = new()
        {
            Font = new("Arial", 12, FontStyle.Bold),
            ForeColor = Color.Transparent,
            Dock = DockStyle.Bottom,
            TextAlign = ContentAlignment.BottomCenter,
            Text = "Loading author..."
        };
        Label attributeLabel = new()
        {
            Font = new("Arial", 12, FontStyle.Bold),
            ForeColor = Color.Transparent,
            Dock = DockStyle.Bottom,
            TextAlign = ContentAlignment.BottomCenter,
            Text = "https://zenquotes.io/"
        };
        Loop(new HttpClient(), quoteLabel, authorLabel);

        Controls.Add(authorLabel);
        Controls.Add(attributeLabel);
        Controls.Add(quoteLabel);
    }
}
