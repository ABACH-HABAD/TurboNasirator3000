using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using TurboNasiratorClasses.Computations;
using TurboNasiratorClasses.Create;
using TurboNasiratorClasses.Delete;
using TurboNasiratorClasses.RandomElements;
using TurboNasiratorClasses.Event;
using TurboNasiratorClasses.Pack;
using TurboNasiratorClasses.Unpack;


namespace TurboNasirator3000;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public const string WHAT_IS_IT = "Турбо Насиратор 3000 - это программа позволяющая создать много папок вложенных друг в друга.\nДля чего это нужно? Если попытаться удалить начальную папку, то windows выдаст ошибку о невозможности её удалить. Соответственно в такой папке можно прятать ценную информацию, ведь никто в здравом уме не полезет открывать десятки тысяч вложенных друг в друга папок.\n\n Укажи путь к месту где нужно насрать папками, а так же путь к папке которую нужно спрятать среди них, а так же ключ, по которому ты можешь найти нужные данные";
    public const string WHAT_IS_KEY = "Ключ - это набор цифр, указывающих на номера папок в которых будет хранится упакованная информация.\nНапример ключ 2568 будет означать, что вам необходимо открыть сначала папку 2, затем 5, потом 6 и в конце 8, если считать сверху вниз";

    private readonly IProgression _progression;
    private readonly IKeyGenerator _keyGenerator;
    private readonly ICreate _createService;
    private readonly IDelete _deleteService;
    private readonly IPack _packService;
    private readonly IUnpack _unpackService;

    private bool isCreatedStarted;
    private bool isDeletedStarted;
    private bool isPackStarted;
    private bool isUnpackStarted;

    public MainWindow()
    {
        InitializeComponent();

        _progression = App.Services.GetService<IProgression>() ?? throw new Exception($"Служба {typeof(IProgression)} не обнаружена");
        _keyGenerator = App.Services.GetService<IKeyGenerator>() ?? throw new Exception($"Служба {typeof(IKeyGenerator)} не обнаружена");
        _createService = App.Services.GetService<ICreate>() ?? throw new Exception($"Служба {typeof(ICreate)} не обнаружена");
        _deleteService = App.Services.GetService<IDelete>() ?? throw new Exception($"Служба {typeof(IDelete)} не обнаружена");
        _packService = App.Services.GetService<IPack>() ?? throw new Exception($"Служба {typeof(IPack)} не обнаружена");
        _unpackService = App.Services.GetService<IUnpack>() ?? throw new Exception($"Служба {typeof(IUnpack)} не обнаружена");

        _createService.ProgressChanged += BarHandler;
        _deleteService.ProgressChanged += BarHandler;
        _packService.ProgressChanged += BarHandler;
        _unpackService.ProgressChanged += BarHandler;

        StartPathCreate.Text = $@"C:\Users\{Environment.UserName}\Desktop\ТУТ НАСРАНО";
        StartPathDelete.Text = $@"C:\Users\{Environment.UserName}\Desktop\ТУТ НАСРАНО";
        StartPathPack.Text = $@"C:\Users\{Environment.UserName}\Desktop\ТУТ НАСРАНО";
        StartPathUnpack.Text = $@"C:\Users\{Environment.UserName}\Desktop\ТУТ НАСРАНО";

        UnpackDirectorty.Text = $@"C:\Users\{Environment.UserName}\Desktop\Распаковка";

        ClearProgress();
    }

    public async Task Create()
    {
        if (isCreatedStarted || isDeletedStarted || isPackStarted || isUnpackStarted)
        {
            MessageBox.Show("Процесс уже запущено");
            return;
        }

        ClearProgress();

        if (!int.TryParse(DeepFolders.Text, out int deep))
        {
            MessageBox.Show("Введите глубину папок");
            return;
        }
        if (!int.TryParse(FoldersInFolder.Text, out int folderInFolder))
        {
            MessageBox.Show("Введите количество папок в папке");
            return;
        }

        if (StartPathCreate.Text == string.Empty)
        {
            MessageBox.Show("Введите путь к папке");
            return;
        }

        isCreatedStarted = true;

        Queue<string> paths = await _createService.PrepareAllStrings(path: StartPathCreate.Text, octaves: deep, countIncluded: folderInFolder);
        await _createService.CreateFolders(baseDirectory: StartPathCreate.Text, foldersQueue: paths);

        MessageBox.Show("Выполнено!");
        isCreatedStarted = false;
    }

    public async Task Delete()
    {
        if (isCreatedStarted || isDeletedStarted || isPackStarted || isUnpackStarted)
        {
            MessageBox.Show("Процесс уже запущено");
            return;
        }

        ClearProgress();

        if (StartPathDelete.Text == string.Empty)
        {
            MessageBox.Show("Введите путь к папке");
            return;
        }

        isDeletedStarted = true;

        DeleteFoldersCount.Text = "Папок обнаружено: 0";
        DeleteFoldersCount.Height = 24;
        Stack<string> paths = await _deleteService.PrepareAllStrings(StartPathDelete.Text);
        await _deleteService.DeleteFolders(foldersStack: paths);

        MessageBox.Show("Выполнено!");
        isDeletedStarted = false;
    }

    public async Task Pack()
    {
        if (isCreatedStarted || isDeletedStarted || isPackStarted || isUnpackStarted)
        {
            MessageBox.Show("Процесс уже запущено");
            return;
        }

        isPackStarted = true;

        ClearProgress();

        if (IncludedDirectorty.Text != string.Empty)
        {
            if (KeyBoxPack.Text == string.Empty) GenerateKey();

            if (!int.TryParse(KeyBoxPack.Text, out _))
            {
                MessageBox.Show("Ключ должен быть числом");
                isPackStarted = false;
                return;
            }

            try
            {
                await _packService.PackData(StartPathPack.Text, IncludedDirectorty.Text, KeyBoxPack.Text);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        else
        {
            MessageBox.Show("Укажите путь для упаковки");
        }

        MessageBox.Show("Выполнено!");
        isPackStarted = false;
    }

    public async Task Unpack()
    {
        if (isCreatedStarted || isDeletedStarted || isPackStarted || isUnpackStarted)
        {
            MessageBox.Show("Процесс уже запущено");
            return;
        }

        isUnpackStarted = true;

        ClearProgress();

        if (UnpackDirectorty.Text != string.Empty)
        {
            if (KeyBoxUnpack.Text == string.Empty)
            {
                MessageBox.Show("Укажите ключ");
                isUnpackStarted = false;
                return;
            }
            else if (!int.TryParse(KeyBoxUnpack.Text, out _))
            {
                MessageBox.Show("Ключ должен быть числом");
                isUnpackStarted = false;
                return;
            }

            try
            {
                await _unpackService.UnpackData(StartPathUnpack.Text, UnpackDirectorty.Text, KeyBoxUnpack.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        else
        {
            MessageBox.Show("Укажите путь для распаковки");
        }

        MessageBox.Show("Выполнено!");
        isUnpackStarted = false;
    }

    private async void StartCreate_Click(object sender, RoutedEventArgs e) => await Create();
    private async void StartDelete_Click(object sender, RoutedEventArgs e) => await Delete();
    private async void StartPack_Click(object sender, RoutedEventArgs e) => await Pack();
    private async void StartUnpack_Click(object sender, RoutedEventArgs e) => await Unpack();

    private void WhatIsIt_Click(object sender, RoutedEventArgs e) => MessageBox.Show(WHAT_IS_IT);
    private void KeyButton_Click(object sender, RoutedEventArgs e) => MessageBox.Show(WHAT_IS_KEY);
    private void GenerateKey_Click(object sender, RoutedEventArgs e) => GenerateKey();
    private void FoldersInFolder_TextChanged(object sender, TextChangedEventArgs e) => Calculate();
    private void DeepFolders_TextChanged(object sender, TextChangedEventArgs e) => Calculate();

    private void Calculate()
    {

        if (int.TryParse(DeepFolders.Text, out int deep) && int.TryParse(FoldersInFolder.Text, out int folderInFolder))
        {
            AllFolders.Text = $"{_progression.Sum(number: deep, denominator: folderInFolder)}";
        }
        else AllFolders.Text = string.Empty;
    }

    private void GenerateKey()
    {
        (int keyLength, int folderCount) = _keyGenerator.StringPrepare(FoldersInFolder.Text);
        KeyBoxPack.Text = _keyGenerator.GenerateKey(keyLength, folderCount);
    }

    private void ClearProgress()
    {
        FirstBlockCreate.Text = IProgressCalculation.DOES_NOT_STARTED;
        SecondBlockCreate.Text = IProgressCalculation.DOES_NOT_STARTED;
        FirstBlockDelete.Text = IProgressCalculation.DOES_NOT_STARTED;
        SecondBlockDelete.Text = IProgressCalculation.DOES_NOT_STARTED;
        BlockPack.Text = IProgressCalculation.DOES_NOT_STARTED;
        BlockUncap.Text = IProgressCalculation.DOES_NOT_STARTED;

        FirstBarCreate.Value = 0;
        SecondBarCreate.Value = 0;
        FirstBarDelete.Value = 0;
        SecondBarDelete.Value = 0;
        BarPack.Value = 0;
        BarUnpack.Value = 0;

        DeleteFoldersCount.Text = string.Empty;
        DeleteFoldersCount.Height = 0;
    }

    private void BarHandler(object? sender, ProgressChangedEventArgs e)
    {
        Dispatcher.Invoke(() =>
        {
            switch (e.ProcessID)
            {
                case ProgressChangedEventArgs.Progresses.FirstCreate:
                    FirstBarCreate.Value = e.Persentage;
                    FirstBlockCreate.Text = e.Status;
                    break;
                case ProgressChangedEventArgs.Progresses.SecondCreate:
                    SecondBarCreate.Value = e.Persentage;
                    SecondBlockCreate.Text = e.Status;
                    break;
                case ProgressChangedEventArgs.Progresses.FirstDelete:
                    if (e.Persentage == -1) FirstBarDelete.IsIndeterminate = true;
                    else FirstBarDelete.IsIndeterminate = false;

                    DeleteFoldersCount.Text = $"Папок обнаружено: {e.FoldersCount}";

                    FirstBarDelete.Value = e.Persentage;
                    FirstBlockDelete.Text = e.Status;
                    break;
                case ProgressChangedEventArgs.Progresses.SecondDelete:
                    SecondBarDelete.Value = e.Persentage;
                    SecondBlockDelete.Text = e.Status;
                    break;
                case ProgressChangedEventArgs.Progresses.Pack:
                    BarPack.Value = e.Persentage;
                    BlockPack.Text = e.Status;
                    break;
                case ProgressChangedEventArgs.Progresses.Unpack:
                    BarUnpack.Value = e.Persentage;
                    BlockUncap.Text = e.Status;
                    break;
            }
        });
    }
}