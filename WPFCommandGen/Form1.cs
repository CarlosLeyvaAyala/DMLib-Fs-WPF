using DMLib_WPF.CmdGen;
using TextCopy;

namespace WPFCommandGen {
    public partial class Form1 : Form {
        private Generator g = new();
        public Form1() {
            InitializeComponent();
        }

        private void BtnGen_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(tbFsClassName.Text)
                || string.IsNullOrWhiteSpace(tbCommands.Text)
                || string.IsNullOrWhiteSpace(cbWPFClassType.Text)) return;

            g.Generate(cbWPFClassType.Text, tbFsClassName.Text, tbCommands.Text);
            tbWpfOutput.Text = g.XAML;
            tbFsOutput.Text = g.Fs;
            tbCsOutput.Text = g.Cs;
        }

        private void BtnDeclFromClpb_Click(object sender, EventArgs e) {
            var t = ClipboardService.GetText();
            tbFsClassName.Text = g.GetFsClass(t);
            tbCommands.Text = g.GetFsCommands(t);
        }
    }
}
