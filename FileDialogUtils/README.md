# FileDialogUtils

Lightweight utility library for working with Windows file dialogs and generating safe, timestamped file names.

---

## Installation

You can install the package via NuGet:

```bash
dotnet add package FileDialogUtils
```
---

## Usage

```csharp

using FileDialogUtils;

// Open File Dialog with default Documents folder
var openDialog = FileDialogUtility.GetOpenFileDialog();

// Save File Dialog with custom file name, extension, and filter
var saveDialog = FileDialogUtility.GetSaveFileDialog(
    fileName: "Report",
    fileExtension: ".xlsx",
    filter: "Excel Files|*.xlsx"
);

if (saveDialog.ShowDialog() == DialogResult.OK)
{
    File.WriteAllText(saveDialog.FileName, "Your content here");
}
```
