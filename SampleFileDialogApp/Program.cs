using FileDialogUtils;
using System.Windows.Forms;

class Program
{
    [STAThread]
    static void Main()
    {
        var dialog = FileDialogUtility.GetSaveFileDialog(
            directoryPath: "New",
            fileName: "report",
            fileExtension: ".txt",
            filter: "Text Files|*.txt"
        );

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            File.WriteAllText($"{dialog.FileName}", "Hello, this is saved content!");
        }
        else
        {
            Console.WriteLine("User cancelled the dialog.");
        }
    }
}
