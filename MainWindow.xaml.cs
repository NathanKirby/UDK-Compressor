using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

using Microsoft.Win32;
using System.IO;

using RlUpk.Core.Classes.Compression;
using RlUpk.Core.Serialization.Default;

namespace UDKCompressionWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string? selectedFilePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ResetFileSelection()
        {
            CompressButton.IsEnabled = false;
            FilePathText.Text = "No file selected.";
            selectedFilePath = null;
        }

        private void CompressClick(object sender, RoutedEventArgs e)
        {
            if (selectedFilePath == null)
            {
                MessageBox.Show("No file has been selected to compress!");
                ResetFileSelection();
                return;
            }

            if (!File.Exists(selectedFilePath))
            {
                MessageBox.Show("Selected file nolonger exists!");
                ResetFileSelection();
                return;
            }

            string inputStem = System.IO.Path.GetFileNameWithoutExtension(selectedFilePath);
            if (inputStem.EndsWith("_compressed"))
            {
                MessageBox.Show("Selected file has already been compressed!");
                ResetFileSelection();
                return;
            }

            string? inputParentDirectory = System.IO.Path.GetDirectoryName(selectedFilePath);
            if (inputParentDirectory == null)
            {
                MessageBox.Show("Selected file has an invalid parent directory!");
                ResetFileSelection();
                return;
            }

            // Get input file size in bytes and ensure it's not empty
            long inputFileBytes = new FileInfo(selectedFilePath).Length;
            if (inputFileBytes <= 0)
            {
                MessageBox.Show("Selected file is empty!");
                ResetFileSelection();
                return;
            }

            // Build output file path and handle path occupied case
            string outputFilePath = inputParentDirectory + "\\" + inputStem + "_compressed.udk";
            if (File.Exists(outputFilePath))
            {
                var selection = MessageBox.Show($"Would you like to replace {outputFilePath}?", "File already exists!", MessageBoxButton.YesNo);
                if (selection == MessageBoxResult.Yes)
                {
                    File.Delete(outputFilePath);
                }
                else if (selection == MessageBoxResult.No)
                {
                    ResetFileSelection();
                    return;
                }
            }

            FilePathText.Text = "Compressing...";
            CompressButton.IsEnabled = false;
            ChooseButton.IsEnabled = false;

            // Compress the package
            try
            {
                var headerSerializer = FileSummarySerializer.GetDefaultSerializer();
                var exportTableItemSerializer = new ExportTableItemSerializer(new FNameSerializer(), new ObjectIndexSerializer(), new FGuidSerializer());
                using var inputPacketStream = File.OpenRead(selectedFilePath);
                using var outputStream = File.Create(outputFilePath);
                var compressor = new PackageCompressor(headerSerializer, exportTableItemSerializer, new FCompressedChunkinfoSerializer());
                compressor.CompressFile(inputPacketStream, outputStream);
            }
            catch (InvalidDataException)
            {
                File.Delete(outputFilePath);

                MessageBox.Show("Selected file has already been compressed! The trailing '_compressed' was not found.");

                ResetFileSelection();
                return;
            }
            catch (Exception ex)
            {
                File.Delete(outputFilePath);

                if (MessageBox.Show($"An exception has occurred. Copy info?", "Error!", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Clipboard.SetText(ex.ToString());
                }

                ResetFileSelection();
                return;
            }
            finally
            {
                FilePathText.Text = System.IO.Path.GetFileName(selectedFilePath); ;
                CompressButton.IsEnabled = true;
                ChooseButton.IsEnabled = true;
            }

            if (!File.Exists(outputFilePath))
            {
                MessageBox.Show("Output file failed to write. Try running as administrator.");
                ResetFileSelection();
                return;
            }
            
            long outputFileBytes = new FileInfo(outputFilePath).Length;
            long fileDifference = inputFileBytes - outputFileBytes;

            string fileDifferenceMb = ((double)fileDifference / 1024 / 1024).ToString("F2").TrimStart('-');
            string compressionRatio = (((double)(inputFileBytes - outputFileBytes) / inputFileBytes) * 100).ToString("F2") + "%";

            if (fileDifference < 0) // Output is larger than input
            {
                MessageBox.Show($"Somehow, the output was larger than the input by {fileDifferenceMb} MB. What the hell did you do?", "Well shit.");
            }
            else if (fileDifference == 0)
            {
                MessageBox.Show($"For some reason, no compression was able to be done on this file. Is it already compressed?", "Well shit.");
            }
            else if (fileDifference > 0)
            {
                var selection = MessageBox.Show($"Input '{inputStem}' size has been reduced by {compressionRatio} ({fileDifferenceMb} MB) and saved as '{inputStem + "_compressed"}'! Open output folder?", "Success!", MessageBoxButton.YesNo);
                if (selection == MessageBoxResult.Yes)
                {
                    Process.Start("explorer.exe", inputParentDirectory);
                }
            }
        }

        private void ChooseClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new()
            {
                Filter = "UDK/UPK files (*.udk;*.upk)|*.udk;*.upk|All files (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                selectedFilePath = dialog.FileName;
                FilePathText.Text = System.IO.Path.GetFileName(selectedFilePath);
                CompressButton.IsEnabled = true;
            }
        }

        private void MartinnClick(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/Martinii89",
                UseShellExecute = true
            });
        }

        private void LicenseClick(object sender, MouseButtonEventArgs e)
        {
            string licensePath = System.IO.Path.Combine(AppContext.BaseDirectory, "license.txt");
            if (!File.Exists(licensePath)) {
                File.WriteAllText(licensePath, License.GetLicense());
            }
            Process.Start("notepad.exe", licensePath);
        }
    }
}