using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.CustomProperties;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Validation;
using Alignment = DocumentFormat.OpenXml.Spreadsheet.Alignment;
using BottomBorder = DocumentFormat.OpenXml.Spreadsheet.BottomBorder;
using Color = DocumentFormat.OpenXml.Spreadsheet.Color;
using Fill = DocumentFormat.OpenXml.Spreadsheet.Fill;
using Font = DocumentFormat.OpenXml.Spreadsheet.Font;
using Fonts = DocumentFormat.OpenXml.Spreadsheet.Fonts;
using ForegroundColor = DocumentFormat.OpenXml.Spreadsheet.ForegroundColor;
using HorizontalAlignmentValues = DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues;
using Hyperlink = DocumentFormat.OpenXml.Spreadsheet.Hyperlink;
using LeftBorder = DocumentFormat.OpenXml.Spreadsheet.LeftBorder;
using PatternFill = DocumentFormat.OpenXml.Spreadsheet.PatternFill;
using RightBorder = DocumentFormat.OpenXml.Spreadsheet.RightBorder;
using Text = DocumentFormat.OpenXml.Spreadsheet.Text;
using TopBorder = DocumentFormat.OpenXml.Spreadsheet.TopBorder;
using Underline = DocumentFormat.OpenXml.Spreadsheet.Underline;
using VerticalAlignmentValues = DocumentFormat.OpenXml.Spreadsheet.VerticalAlignmentValues;

namespace ExcelManager
{
    /// <summary>
    /// Класс строки
    /// </summary>
    public class MRow
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="r">Строка</param>
        public MRow(Row r)
        {
            this.r = r;
        }
        /// <summary>
        /// Индекс строки
        /// </summary>
        public uint RowIndex => r.RowIndex.Value;
        /// <summary>
        /// Высота строки
        /// </summary>
        public double Height
        {
            get
            {
                return r.Height;
            }
            set
            {
                r.Height = new DoubleValue(value);
                r.CustomHeight = new BooleanValue(true);
            }
        }
        /// <summary>
        /// Строка
        /// </summary>
        public Row r { get; set; }
        /// <summary>
        /// Связанные диапазоны
        /// </summary>
        public List<MMergeCell> MMergeCells = new List<MMergeCell>();


    }
    /// <summary>
    /// Класс региона
    /// </summary>
    public class MMergeCell
    {
        public MergeCell MergeCell { get; set; }
        public CellRangeAddress CRA { get; set; }
    }
    /// <summary>
    /// Выравнивание по горизонтали
    /// </summary>
    public enum HorizontalAlignmentV
    {
        /// <summary>
        /// Левое
        /// </summary>
        Left = 1,
        /// <summary>
        /// Правое
        /// </summary>
        Right = 2,
        /// <summary>
        /// Центр
        /// </summary>
        Center = 3
    }
    /// <summary>
    /// Выравнивание по вертикали
    /// </summary>
    public enum VerticalAlignmentV
    {
        /// <summary>
        /// Центр
        /// </summary>
        Center,
        /// <summary>
        /// Вверх
        /// </summary>
        Top,
        /// <summary>
        /// Вниз
        /// </summary>
        Bottom
    }
    /// <summary>
    /// Тип границы
    /// </summary>
    public enum BorderValues
    {
        /// <summary>
        /// Нет
        /// </summary>
        None,
        /// <summary>
        /// Тонкая линия
        /// </summary>
        Thin
    }
    /// <summary>
    /// Класс определяющий параметры текста
    /// </summary>
    public class FontOpenXML
    {
        /// <summary>
        /// Имя шрифта
        /// </summary>
        public string fontname { get; set; }
        /// <summary>
        /// Цвет
        /// </summary>
        public System.Drawing.Color color { get; set; }
        /// <summary>
        /// Размер
        /// </summary>
        public double size { get; set; }
        /// <summary>
        /// Толщина
        /// </summary>
        public bool Bold { get; set; }
        /// <summary>
        /// Курсив
        /// </summary>
        public bool Italic { get; set; }
        /// <summary>
        /// Перенос по строкам
        /// </summary>
        public bool wordwrap { get; set; }
        /// <summary>
        /// Выравнивание по горизонтали
        /// </summary>
        public HorizontalAlignmentV HorizontalAlignment { get; set; }
        /// <summary>
        /// Выравнивание по вертикали
        /// </summary>
        public VerticalAlignmentV VerticalAlignment { get; set; }
        /// <summary>
        /// Формат строки
        /// </summary>
        public uint Format { get; set; } = 0;
        /// <summary>
        /// Подчеркивание
        /// </summary>
        public bool Underline { get; set; } = false;
        /// <summary>
        /// Конструктор
        /// </summary>
        public FontOpenXML()
        {
            fontname = "Arial";
            color = System.Drawing.Color.Black;
            size = 10;
            Bold = false;
            Italic = false;
            wordwrap = false;
            HorizontalAlignment = HorizontalAlignmentV.Left;
            VerticalAlignment = VerticalAlignmentV.Center;
        }
    }
    /// <summary>
    /// Класс определяющий параметры границы
    /// </summary>
    public class BorderOpenXML
    {
        /// <summary>
        /// Левая граница
        /// </summary>
        public BorderValues LeftBorder { get; set; } = BorderValues.Thin;
        /// <summary>
        /// Правая граница
        /// </summary>
        public BorderValues RightBorder { get; set; } = BorderValues.Thin;
        /// <summary>
        /// Верхняя граница
        /// </summary>
        public BorderValues TopBorder { get; set; } = BorderValues.Thin;
        /// <summary>
        /// Нижняя граница
        /// </summary>
        public BorderValues BottomBorder { get; set; } = BorderValues.Thin;

    }
    /// <summary>
    /// Класс определяющий параметры заливки
    /// </summary>
    public class FillOpenXML
    {
        /// <summary>
        /// Цвет
        /// </summary>
        public System.Drawing.Color color;
        /// <summary>
        /// Конструктор
        /// </summary>
        public FillOpenXML()
        {
            color = System.Drawing.Color.Transparent;
        }

    }
    /// <summary>
    /// Числовой формат
    /// </summary>
    public enum DefaultNumFormat
    {
        /// <summary>
        /// General
        /// </summary>
        F0 = 0,
        /// <summary>
        /// 0
        /// </summary>
        F1 = 1,
        /// <summary>
        /// 0.00
        /// </summary>
        F2 = 2,
        /// <summary>
        /// #,##0
        /// </summary>
        F3 = 3,
        /// <summary>
        /// #,##0.00
        /// </summary>
        F4 = 4,
        /// <summary>
        /// 0%
        /// </summary>
        F9 = 9,
        /// <summary>
        /// 0.00%
        /// </summary>
        F10 = 10,
        /// <summary>
        /// 0.00E+00
        /// </summary>
        F11 = 11,
        /// <summary>
        /// # ?/?
        /// </summary>
        F12 = 12,
        /// <summary>
        /// # ??/??
        /// </summary>
        F13 = 13,
        /// <summary>
        /// mm-dd-yy
        /// </summary>
        F14 = 14,
        /// <summary>
        /// d-mmm-yy
        /// </summary>
        F15 = 15,
        /// <summary>
        /// d-mmm
        /// </summary>
        F16 = 16,

        /// <summary>
        /// mmm-yy
        /// </summary>
        F17 = 17,
        /// <summary>
        /// h:mm AM/PM
        /// </summary>
        F18 = 18,
        /// <summary>
        /// h:mm:ss AM/PM
        /// </summary>
        F19 = 19,
        /// <summary>
        /// h:mm
        /// </summary>
        F20 = 20,
        /// <summary>
        /// h:mm:ss
        /// </summary>
        F21 = 21,
        /// <summary>
        /// m/d/yy h:mm
        /// </summary>
        F22 = 22,
        /// <summary>
        /// #,##0 ;(#,##0)
        /// </summary>

        F37 = 37,
        /// <summary>
        /// #,##0 ;[Red](#,##0)
        /// </summary>
        F38 = 38,
        /// <summary>
        /// #,##0.00;(#,##0.00)
        /// </summary>
        F39 = 39,
        /// <summary>
        /// #,##0.00;[Red](#,##0.00)
        /// </summary>
        F40 = 40,
        /// <summary>
        /// _("$"* #,##0.00_);_("$"* \(#,##0.00\);_("$"* "-"??_);_(@_)
        /// </summary>
        F44 = 44,
        /// <summary>
        /// mm:ss
        /// </summary>
        F45 = 45,
        /// <summary>
        /// [h]:mm:ss
        /// </summary>
        F46 = 46,
        /// <summary>
        /// mmss.0
        /// </summary>
        F47 = 47,
        /// <summary>
        /// ##0.0E+0
        /// </summary>
        F48 = 48,
        /// <summary>
        /// @
        /// </summary>
        F49 = 49,

        /// <summary>
        /// [$-404]e/m/d
        /// </summary>
        F27 = 27,
        /// <summary>
        /// m/d/yy
        /// </summary>
        F30 = 30,
        /// <summary>
        /// [$-404]e/m/d
        /// </summary>
        F36 = 36,
        /// <summary>
        /// [$-404]e/m/d
        /// </summary>
        F50 = 50,
        /// <summary>
        /// [$-404]e/m/d
        /// </summary>
        F57 = 57,
        /// <summary>
        /// t0
        /// </summary>
        F59 = 59,
        /// <summary>
        /// t0.00
        /// </summary>
        F60 = 60,
        /// <summary>
        /// t#,##0
        /// </summary>
        F61 = 61,
        /// <summary>
        /// t#,##0.00
        /// </summary>
        F62 = 62,
        /// <summary>
        /// t0%
        /// </summary>
        F67 = 67,
        /// <summary>
        /// t0.00%
        /// </summary>
        F68 = 68,
        /// <summary>
        /// t# ?/?
        /// </summary>
        F69 = 69,
        /// <summary>
        /// t# ??/??
        /// </summary>
        F70 = 70
    }
    /// <summary>
    /// Колонка
    /// </summary>
    public class ColumnOpenXML
    {
        /// <summary>
        /// Колонка
        /// </summary>
        public Column Col { get; set; }
        /// <summary>
        /// Авторазмер
        /// </summary>
        public bool AutoFit { get; set; }
    }
    /// <summary>
    /// Именованный диапазон
    /// </summary>
    public class DefinedNameValue
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Диапазон
        /// </summary>
        public CellRangeAddress cra { get; set; }
    }
    /// <summary>
    /// Класс для работы с Excel файлов
    /// </summary>
    public class ExcelOpenXML : IDisposable
    {
        WorkbookPart workbookPart;
        SheetData sheetData;
        Sheet CurrentSchet;
        WorksheetPart worksheetPart;
        SpreadsheetDocument document;
        WorkbookStylesPart stylesPart;

        public void MarkAsFinal(bool value)
        {
            var cfpp = document.CustomFilePropertiesPart ?? document.AddCustomFilePropertiesPart();
            if (cfpp.Properties == null)
                cfpp.Properties = new Properties();
            if (cfpp.Properties.NamespaceDeclarations.Count(x => x.Key == "vt") == 0)
                cfpp.Properties.AddNamespaceDeclaration("vt", "http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes");

            var _MarkAsFinal = cfpp.Properties.Elements<CustomDocumentProperty>().FirstOrDefault(x => x.Name == "_MarkAsFinal");
            if (_MarkAsFinal == null && value)
            {
                _MarkAsFinal = new CustomDocumentProperty() { FormatId = "{D5CDD505-2E9C-101B-9397-08002B2CF9AE}", PropertyId = 2, Name = "_MarkAsFinal" };
                var vtBool1 = new DocumentFormat.OpenXml.VariantTypes.VTBool { Text = value ? "true" : "false" };
                _MarkAsFinal.AppendChild(vtBool1);
                cfpp.Properties.AddChild(_MarkAsFinal);
            }
            if (_MarkAsFinal != null && !value)
            {
                _MarkAsFinal.Remove();
            }
        }



        /// <summary>
        /// Валидация документа WORD
        /// </summary>
        /// <param name="filepath">ПУть к файлу</param>
        /// <returns></returns>
        public static string ValidateWordDocument(string filepath)
        {
            using (var SD = SpreadsheetDocument.Open(filepath, true))
            {
                var sb = new StringBuilder();
                try
                {
                    var validator = new OpenXmlValidator();
                    var count = 0;
                    foreach (var error in
                        validator.Validate(SD))
                    {
                        count++;
                        sb.AppendLine("Error " + count);
                        sb.AppendLine("Description: " + error.Description);
                        sb.AppendLine("ErrorType: " + error.ErrorType);
                        sb.AppendLine("Node: " + error.Node);
                        sb.AppendLine("Path: " + error.Path.XPath);
                        sb.AppendLine("Part: " + error.Part.Uri);
                        sb.AppendLine("-------------------------------------------");
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                SD.Close();
                return sb.ToString();
            }
        }
        /// <summary>
        /// Добавить именованный диапазон
        /// </summary>
        /// <param name="Name">Имя</param>
        /// <param name="cra">Диапазон</param>
        public void AddDefinedName(string Name, CellRangeAddress cra)
        {
            var definedNamesCol = workbookPart.Workbook.DefinedNames;
            if (definedNamesCol == null)
            {
                workbookPart.Workbook.DefinedNames = new DefinedNames();
            }
            var SheetName = CurrentSchet.Name;
            var definedName = new DefinedName()
            { Name = Name, Text = $"{SheetName}!${GetColumnAddr(cra.From.Cell)}${cra.From.Row}:${GetColumnAddr(cra.To.Cell)}${cra.To.Row}" };
            // Create a new range
            workbookPart.Workbook.DefinedNames.Append(definedName);
        }
        /// <summary>
        /// Получить именованные диапазоны
        /// </summary>
        /// <returns></returns>
        public List<DefinedNameValue> GetDefinedName()
        {
            return workbookPart.Workbook.DefinedNames != null ? workbookPart.Workbook.DefinedNames.Elements<DefinedName>().Select(item => new DefinedNameValue { Name = item.Name, cra = GetCRA(item.Text) }).ToList() : new List<DefinedNameValue>();
        }


        private void ReadParam()
        {
            ReadColumn();
            ReadMergeCells();
            ReadRows();
        }

        private void ClearParam()
        {
            Columns = null;
            cols = null;
            rowsDictionary = null;
            MergeCells = null;
            MergeCellsFile = null;
        }
        private CellRangeAddress GetCRA(string Addres)
        {
            var r = new Regex("^(?<List>.*)[!][$](?<Col1>.*)[$](?<Row1>.*)[:][$](?<Col2>.*)[$](?<Row2>.*)$");
            var m = r.Match(Addres);
            if (m.Success)
            {
                return new CellRangeAddress(Convert.ToUInt32(m.Groups["Row1"].Value), GetColumnIndex(m.Groups["Col1"].Value), Convert.ToUInt32(m.Groups["Row2"].Value), GetColumnIndex(m.Groups["Col2"].Value), m.Groups["List"].Value);
            }
            r = new Regex("^(?<List>.*)[!][$](?<Col1>.*)[$](?<Row1>.*)$");
            m = r.Match(Addres);
            if (m.Success)
            {
                return new CellRangeAddress(Convert.ToUInt32(m.Groups["Row1"].Value), GetColumnIndex(m.Groups["Col1"].Value), Convert.ToUInt32(m.Groups["Row1"].Value), GetColumnIndex(m.Groups["Col1"].Value), m.Groups["List"].Value);
            }

            return null;
        }
        /// <summary>
        /// Колонки
        /// </summary>
        public Dictionary<string, ColumnOpenXML> Columns { get; set; }
        private Columns cols;
        private ColumnOpenXML GetColumn(string Addr)
        {
            if (Columns == null)
                ReadColumn();
            if (Columns.ContainsKey(Addr))
            {
                return Columns[Addr];
            }
            var c = new Column();
            c.Min = c.Max = GetColumnIndex(Addr);
            c.Width = 10;
            cols.Append(c);
            var copx = new ColumnOpenXML() { Col = c, AutoFit = false };
            Columns.Add(Addr, copx);
            return copx;
        }
        private void ReadColumn()
        {
            //Колонки
            Columns = new Dictionary<string, ColumnOpenXML>();
            cols = worksheetPart.Worksheet.GetFirstChild<Columns>();
            //Колонки
            if (cols == null)
            {
                cols = new Columns();
                //Добавляем колонки и ячейки
                worksheetPart.Worksheet.InsertBefore(cols, worksheetPart.Worksheet.Elements<SheetData>().FirstOrDefault());
            }
            foreach (var openXmlElement in cols)
            {
                var col = (Column)openXmlElement;
                var Addr = GetColumnAddr(col.Min);
                if (Columns.ContainsKey(Addr)) continue;
                var copx = new ColumnOpenXML { Col = col, AutoFit = false };
                Columns.Add(Addr, copx);
            }
        }
        Dictionary<uint, MRow> rowsDictionary = new Dictionary<uint, MRow>();
        private void ReadRows()
        {
            rowsDictionary = new Dictionary<uint, MRow>();
            foreach (var row in sheetData.Elements<Row>().ToList())
            {
                rowsDictionary.Add(row.RowIndex, new MRow(row));
            }

            if (MergeCells != null)
            {
                foreach (var merge in MergeCells)
                {
                    AddToRowMergeCell(merge);
                }
            }
        }
        private void AddMRow(MRow rNew, MRow BEFORE)
        {
            if (BEFORE == null)
            {
                sheetData.Append(rNew.r);
            }
            else
            {
                sheetData.InsertBefore(rNew.r, BEFORE.r);
            }

            rowsDictionary.Add(rNew.RowIndex, rNew);
            foreach (var m in rNew.MMergeCells)
            {
                AppendMergeCellsFile(m.MergeCell);
            }
        }



        private void AddToRowMergeCell(MMergeCell merge)
        {
            if (rowsDictionary.ContainsKey(merge.CRA.From.Row))
            {
                rowsDictionary[merge.CRA.From.Row].MMergeCells.Add(merge);
            }
        }



        private MergeCells MergeCellsFile { get; set; }

        private void AppendMergeCellsFile(MergeCell mc)
        {
            if (MergeCellsFile == null)
                CreateMergeCells();
            MergeCellsFile.AppendChild(mc);
        }
        public List<MMergeCell> MergeCells { get; set; }
        private void ReadMergeCells()
        {
            var worksheet = worksheetPart.Worksheet;
            if (worksheet.Elements<MergeCells>().Any())
            {
                MergeCellsFile = worksheet.Elements<MergeCells>().First();
                MergeCells = new List<MMergeCell>();

                foreach (var merge in MergeCellsFile.Elements<MergeCell>())
                {
                    var mc = new MMergeCell { MergeCell = merge, CRA = FromMergeRegion(merge.Reference) };
                    MergeCells.Add(mc);
                }
            }
        }

        private void CreateMergeCells()
        {
            var worksheet = worksheetPart.Worksheet;
            MergeCellsFile = new MergeCells();
            if (worksheet.Elements<CustomSheetView>().Any())
                worksheet.InsertAfter(MergeCellsFile, worksheet.Elements<CustomSheetView>().First());
            else
                worksheet.InsertAfter(MergeCellsFile, worksheet.Elements<SheetData>().First());
            MergeCells = new List<MMergeCell>();
        }


        private void AddMergeCells(MMergeCell merge)
        {
            AppendMergeCellsFile(merge.MergeCell);
            AddToRowMergeCell(merge);
        }


        /// <summary>
        /// Строки и ячейки
        /// </summary>


        private Stream fs;
        /// <summary>
        /// Создать новый документ
        /// </summary>
        /// <param name="fileName">Путь</param>
        /// <param name="name_sheet">Имя 1го листа</param>
        public ExcelOpenXML(string fileName, string name_sheet)
        {
            Create(fileName, name_sheet);
        }
        /// <summary>
        /// Создать документ
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        /// <param name="name_sheet">Имя листа</param>
        public void Create(string fileName, string name_sheet)
        {
            var st = new FileStream(fileName, FileMode.Create);
            IsCreate = true;
            CreateDocument(st, name_sheet);
        }

        public ExcelOpenXML(Stream stream, string name_sheet)
        {
            CreateDocument(stream, name_sheet);
        }

        /// <summary>
        /// Удалить защиту листа
        /// </summary>
        public void ClearProtection()
        {
            var worksheet = worksheetPart.Worksheet;
            var sp = worksheet.Elements<SheetProtection>().FirstOrDefault();
            sp?.Remove();
        }

        /// <summary>
        ///  Создать новый документ
        /// </summary>
        /// <param name="stream">Поток</param>
        /// <param name="name_sheet">Имя 1го листа</param>


        /// <summary>
        /// Открыть файл
        /// </summary>
        /// <param name="fileName">Путь</param>
        /// <param name="index">Индекс текущего листа(начиная с 0)</param>
        public void OpenFile(string fileName, int index)
        {
            var st = new FileStream(fileName, FileMode.Open);
            IsCreate = true;
            OpenDocument(st, index);
        }


        /// <summary>
        /// Открыть файл
        /// </summary>
        /// <param name="stream">Поток</param>
        /// <param name="index">Индекс текущего листа(начиная с 0)</param>
        public void OpenFile(Stream stream, int index)
        {
            OpenDocument(stream, index);
        }


        private void OpenDocument(Stream stream, int index)
        {
            fs = stream;
            //Создаем документ
            document = SpreadsheetDocument.Open(fs, true);
            workbookPart = document.WorkbookPart;
            // Создаем лист
            //Список листов
            var sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
            worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheets.GetFirstChild<Sheet>().Id);
            //Лист            
            CurrentSchet = sheets.Elements<Sheet>().ToArray()[index];
            //Ячейки листа
            sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
            // worksheetPart.Worksheet.Append(sheetData);
            //Таблица стилей
            stylesPart = workbookPart.GetPartsOfType<WorkbookStylesPart>().First(); // AddNewPart<WorkbookStylesPart>();
            stylesPart.Stylesheet ??= GenerateDefaultStyleSheet();
            ClearParam();
            ReadParam();
        }

        private void CreateDocument(Stream stream, string name_sheet)
        {
            fs = stream;
            //Создаем документ
            document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, false);
            // Создаем книгу
            workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();
            // Создаем лист
            worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet();
            //Список листов
            var sheets = workbookPart.Workbook.AppendChild(new Sheets());
            //Лист
            var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = name_sheet };
            sheets.Append(sheet);
            //Ячейки листа
            sheetData = new SheetData();
            worksheetPart.Worksheet.Append(sheetData);
            //Таблица стилей
            stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
            stylesPart.Stylesheet = GenerateDefaultStyleSheet();
            ClearParam();
            ReadParam();
        }

        private bool IsCreate = false;



        /// <summary>
        /// Конструктор
        /// </summary>
        public ExcelOpenXML()
        {

        }

        /// <summary>
        /// Имя текущего листа
        /// </summary>
        public string CurrentSheetName
        {
            get
            {
                if (CurrentSchet != null)
                    return CurrentSchet.Name.HasValue ? CurrentSchet.Name.Value : null;
                return null;
            }
        }
        /// <summary>
        /// Освободить ресурсы
        /// </summary>
        public void Dispose()
        {
            if (document != null)
            {
                document.Dispose();
                sstp = null;
                document = null;
                cols = null;
                workbookPart = null;
                sheetData = null;
                sheetData = null;
                CurrentSchet = null;
                worksheetPart = null;
                document = null;
                stylesPart = null;
                ClearParam();
            }
            if (IsCreate)
            {
                fs?.Close();
                fs?.Dispose();
            }

            fs = null;
        }




        private SharedStringTablePart sstp;
        void ReadShared()
        {
            var stringTable = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
            sstp = stringTable ?? document.WorkbookPart.AddNewPart<SharedStringTablePart>();
        }

        private uint InsertSharedStringItem(string text)
        {
            uint i = 0;
            if (sstp == null)
                ReadShared();

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (var item in sstp.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }
                i++;
            }

            // The text does not exist in the part. Create the SharedStringItem and return its index.
            sstp.SharedStringTable.AppendChild(new SharedStringItem(new Text(text)));
            sstp.SharedStringTable.Count = i;
            sstp.SharedStringTable.UniqueCount = i;
            sstp.SharedStringTable.Save();
            return i;
        }

        /// <summary>
        /// Добавить объединение ячеек
        /// </summary>
        /// <param name="addr">Диапазон</param>
        public void AddMergedRegion(CellRangeAddress addr)
        {
            var merge = new MMergeCell();
            merge.CRA = addr;
            merge.MergeCell = new MergeCell { Reference = new StringValue(ToMergeRegion(addr)) };
            AddMergeCells(merge);

            Cell cellBase = null;
            for (var i = addr.From.Row; i <= addr.To.Row; i++)
            {
                var r = GetRow(i);
                for (var j = addr.From.Cell; j <= addr.To.Cell; j++)
                {
                    var currcell = InsertCellInWorksheet(r.r, GetColumnAddr(j) + i);
                    if (cellBase == null)
                    {
                        cellBase = currcell;
                    }
                    else
                    {
                        currcell.StyleIndex = cellBase.StyleIndex;
                    }
                }
            }

        }
        /// <summary>
        /// Установить стиль диапазону
        /// </summary>
        /// <param name="addr">Диапазон</param>
        /// <param name="Style">Стиль</param>
        /// <param name="onlynull">Установить только ячейкам без стиля</param>
        public void SetStyleRange(CellRangeAddress addr, uint Style, bool onlynull = false)
        {
            for (var i = addr.From.Row; i <= addr.To.Row; i++)
            {
                var r = GetRow(i);
                for (var j = addr.From.Cell; j <= addr.To.Cell; j++)
                {
                    var currcell = InsertCellInWorksheet(r.r, GetColumnAddr(j) + i);
                    if (onlynull)
                    {
                        if (currcell.StyleIndex == null)
                        {
                            currcell.StyleIndex = Style;
                        }
                    }
                    else
                    {
                        currcell.StyleIndex = Style;
                    }
                }
            }
        }


        /// <summary>
        /// Установить ширину колонки
        /// </summary>
        /// <param name="addr">Ячейка</param>
        /// <param name="value">Значение</param>
        public void SetColumnWidth(uint addr, double value)
        {
            SetColumnWidth(GetColumnAddr(addr), value);
        }
        /// <summary>
        /// Установить ширину колонки
        /// </summary>
        /// <param name="addr">Ячейка</param>
        /// <param name="value">Значение</param>
        public void SetColumnWidth(string addr, double value)
        {
            var col = GetColumn(addr);
            col.Col.Width = value;
            col.Col.CustomWidth = true;
        }


        public void CreateRow(uint rowIndex, string XML)
        {
            if (!rowsDictionary.ContainsKey(rowIndex))
            {
                var mrow = new MRow(new Row(XML) { RowIndex = rowIndex });
                sheetData.AppendChild(mrow.r);
                rowsDictionary.Add(rowIndex, mrow);
            }
            rowsDictionary[rowIndex].r.InnerXml = XML;
        }

        /// <summary>
        /// Добавить лист
        /// </summary>
        /// <param name="name_sheet">Наименование</param>
        public void AddSheet(string name_sheet)
        {
            // Лист
            worksheetPart = workbookPart.AddNewPart<WorksheetPart>();

            worksheetPart.Worksheet = new Worksheet();
            //Находим список листов
            var sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
            //Указатель на лист
            var relationshipId = workbookPart.GetIdOfPart(worksheetPart);
            // Получаем уникальный ID
            uint sheetId = 1;
            if (sheets.Elements<Sheet>().Any())
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }
            // Новый лист
            CurrentSchet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = name_sheet };
            sheets.Append(CurrentSchet);

            //Колонки + данные о ячейках

            sheetData = new SheetData();
            worksheetPart.Worksheet.Append(sheetData);
            ClearParam();
            ReadParam();

        }
        /// <summary>
        /// Вставить лист в начало
        /// </summary>
        /// <param name="name_sheet">Наименование</param>
        public void InsertSheet(string name_sheet)
        {
            // Лист
            worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet();
            //Находим список листов
            var sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
            //Указатель на лист
            var relationshipId = workbookPart.GetIdOfPart(worksheetPart);
            // Получаем уникальный ID
            uint sheetId = 1;
            if (sheets.Elements<Sheet>().Any())
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }
            // Новый лист
            CurrentSchet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = name_sheet };
            sheets.InsertAt<Sheet>(CurrentSchet, 0);

            //Колонки + данные о ячейках
            sheetData = new SheetData();
            worksheetPart.Worksheet.Append(sheetData);
            ClearParam();
            ReadParam();

        }

        /// <summary>
        /// Установить видимость колонки
        /// </summary>
        /// <param name="cell">Колонка</param>
        /// <param name="visible">Значение</param>
        public void SetColumnVisible(string cell, bool visible)
        {
            GetColumn(cell).Col.Hidden = BooleanValue.FromBoolean(!visible);
        }
        /// <summary>
        /// Сохранить файл и ликвидировать объект
        /// </summary>
        public void Save()
        {
            if (document.WorkbookPart != null)
            {
                document.WorkbookPart.Workbook.CalculationProperties ??= new CalculationProperties();
                document.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;
                document.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;
            }
            if (Columns?.Count == 0)
            {
                cols?.Remove();
            }
           
            //Косяк в 2010 OFFICE при работе с этим тэгом
            var ex = workbookPart.Workbook.GetFirstChild<WorkbookExtensionList>();
            ex?.Remove();
            if (workbookPart.WorkbookStylesPart != null)
            {
                var ex1 = workbookPart.WorkbookStylesPart.Stylesheet.GetFirstChild<StylesheetExtensionList>();
                ex1?.Remove();
            }
            document.Save();
            document.Close();
            Dispose();
        }
        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>
        /// <param name="isSharedString">Использовать таблицу строк</param>
        public void PrintCell(MRow Row, string Cell, string value, uint? styleid, bool isSharedString = false)
        {
            var cell = InsertCellInWorksheet(Row.r, Cell + Row.r.RowIndex.ToString());
            if (isSharedString)
            {
                cell.DataType = CellValues.SharedString;
                if (value != null)
                {
                    cell.CellValue = new CellValue { Text = InsertSharedStringItem(value).ToString() };
                }
            }
            else
            {
                cell.DataType = CellValues.InlineString;
                if (value != null)
                {
                    var istring = new InlineString() { Text = new Text() { Text = value } };
                    cell.InlineString = istring;
                    cell.CellValue = null;
                }
            }


            if (styleid.HasValue)
                cell.StyleIndex = styleid.Value;

        }

        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="RowI">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>
        public void PrintCell(uint RowI, uint Cell, DateTime? value, uint? styleid)
        {
            PrintCell(GetRow(RowI), GetColumnAddr(Cell), value, styleid);
        }
        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="RowI">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>
        public void PrintCell(uint RowI, string Cell, DateTime? value, uint? styleid)
        {
            PrintCell(GetRow(RowI), Cell, value, styleid);
        }
        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>
        public void PrintCell(MRow Row, string Cell, DateTime? value, uint? styleid)
        {
            //  var Row = GetRow(RowI);
            var cell = InsertCellInWorksheet(Row.r, Cell + Row.r.RowIndex);
            cell.DataType = CellValues.Number;
            cell.CellValue = value.HasValue ? new CellValue() { Text = value.Value.Date.ToOADate().ToString() } : new CellValue() { Text = "" };
            if (styleid.HasValue)
                cell.StyleIndex = styleid.Value;

        }
        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>
        public void PrintCell(MRow Row, string Cell, double value, uint? styleid)
        {
            var cell = InsertCellInWorksheet(Row.r, Cell + Row.r.RowIndex);
            cell.DataType = CellValues.Number;
            if (styleid.HasValue)
                cell.StyleIndex = styleid.Value;
            cell.CellValue = new CellValue() { Text = value.ToString().Replace(",", ".") };
        }
      
        public void PrintCell(MRow Row, string Cell, long value, uint? styleid)
        {
            var cell = InsertCellInWorksheet(Row.r, Cell + Row.r.RowIndex);
            cell.DataType = CellValues.Number;
            if (styleid.HasValue)
                cell.StyleIndex = styleid.Value;
            cell.CellValue = new CellValue() { Text = value.ToString().Replace(",", ".") };
        }

      


        /// <summary>
        /// Вставить гиперссылку
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="Uri">Адрес</param>

        public void PrintHyperlink(MRow Row, uint Cell, string Uri)
        {
            Hyperlinks hyperlinks;
            var i = worksheetPart.Worksheet.Descendants<Hyperlinks>().Count();
            // create new Hyperlinks tag if none exists before, else use the same to append new hyperlink
            hyperlinks = i == 0 ? new Hyperlinks() : worksheetPart.Worksheet.Descendants<Hyperlinks>().First();
            var hpid = "HP_" + hyperlinks.Count();
            var children = new Hyperlink { Reference = GetExcelColumnName(Cell) + Row.r.RowIndex.Value, Id = hpid };
            hyperlinks.Append(children);
            if (i == 0)
            {
                var pageMargins = worksheetPart.Worksheet.Descendants<SheetData>().First();
                worksheetPart.Worksheet.InsertAfter(hyperlinks, pageMargins);
            }
            worksheetPart.AddHyperlinkRelationship(new System.Uri(Uri, System.UriKind.RelativeOrAbsolute), true, hpid);
        }
        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>
        /// <param name="isSharedString">Использовать таблицу строк</param>
        public void PrintCell(MRow Row, uint Cell, string value, uint? styleid, bool isSharedString = false)
        {
            PrintCell(Row, GetColumnAddr(Cell), value, styleid, isSharedString);
        }
        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>
        public void PrintCell(uint Row, uint Cell, string value, uint? styleid)
        {
            PrintCell(GetRow(Row), GetColumnAddr(Cell), value, styleid);
        }
        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>
        public void PrintCell(uint Row, uint Cell, int value, uint? styleid)
        {
            PrintCell(GetRow(Row), GetColumnAddr(Cell), value, styleid);
        }
        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>
        public void PrintCell(uint Row, uint Cell, decimal? value, uint? styleid)
        {
            PrintCell(GetRow(Row), Cell, value, styleid);
        }
        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>
        public void PrintCell(uint Row, uint Cell, double? value, uint? styleid)
        {
            PrintCell(GetRow(Row), Cell, value, styleid);
        }
        public void PrintCell(uint Row, uint Cell, long? value, uint? styleid)
        {
            PrintCell(GetRow(Row), Cell, value, styleid);
        }
        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>
        public void PrintCell(MRow Row, uint Cell, DateTime? value, uint? styleid)
        {
            PrintCell(Row, GetColumnAddr(Cell), value, styleid);
        }
        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>
        public void PrintCell(MRow Row, uint Cell, double value, uint? styleid)
        {
            PrintCell(Row, GetColumnAddr(Cell), value, styleid);
        }
        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>
        public void PrintCell(MRow Row, uint Cell, decimal value, uint? styleid)
        {
            PrintCell(Row, GetColumnAddr(Cell), Convert.ToDouble(value), styleid);
        }
        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>
        public void PrintCell(MRow Row, uint Cell, decimal? value, uint? styleid)
        {
            if (value.HasValue)
                PrintCell(Row, GetColumnAddr(Cell), Convert.ToDouble(value), styleid);
            else
                PrintCell(Row, GetColumnAddr(Cell), "", styleid);
        }
        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>
        public void PrintCell(MRow Row, uint Cell, double? value, uint? styleid)
        {
            if (value.HasValue)
                PrintCell(Row, GetColumnAddr(Cell), Convert.ToDouble(value), styleid);
            else
                PrintCell(Row, GetColumnAddr(Cell), "", styleid);
        }

        public void PrintCell(MRow Row, uint Cell, long? value, uint? styleid)
        {
            if (value.HasValue)
                PrintCell(Row, GetColumnAddr(Cell), value.Value, styleid);
            else
                PrintCell(Row, GetColumnAddr(Cell), "", styleid);
        }

        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>
        public void PrintCell(MRow Row, uint Cell, int value, uint? styleid)
        {

            PrintCell(Row, GetColumnAddr(Cell), Convert.ToDouble(value), styleid);
        }


        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>
        public void PrintCell(MRow Row, uint Cell, int? value, uint? styleid)
        {
            if (value.HasValue)
                PrintCell(Row, GetColumnAddr(Cell), value.Value, styleid);
            else
                PrintCell(Row, GetColumnAddr(Cell), "", styleid);
        }
        /// <summary>
        /// Вставить формулу
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="Formula">Формула</param>
        /// <param name="styleid">Стиль</param>
        public void PrintCellFormula(MRow Row, uint Cell, string Formula, uint? styleid)
        {
            PrintCellFormula(Row, GetColumnAddr(Cell), Formula, styleid);
        }
        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Столбец</param>
        /// <param name="Formula">Формула</param>
        /// <param name="styleid">Стиль</param>
        public void PrintCellFormula(MRow Row, string Cell, string Formula, uint? styleid)
        {

            var cell = InsertCellInWorksheet(Row.r, Cell + Row.r.RowIndex);
            cell.DataType = CellValues.Number;
            if (styleid.HasValue)
                cell.StyleIndex = styleid.Value;
            var cellformula = new CellFormula { Text = Formula };
            var cellValue = new CellValue();
            cell.Append(cellformula);
            cell.Append(cellValue);
        }




        private static double GetWidth(string font, double fontSize, string text)
        {
            var stringFont = new System.Drawing.Font(font, Convert.ToSingle(fontSize));
            return GetWidth(stringFont, text);
        }
        private static double GetWidth(System.Drawing.Font stringFont, string text)
        {
            SizeF textSize;
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                textSize = g.MeasureString(text, stringFont);
            }
            double width = (double)(((textSize.Width / (double)7) * 256) - (128 / 7)) / 256;
            width = (double)decimal.Round((decimal)width + 0.5M, 2);

            return width;
        }
      
        /// <summary>
        /// Авторазмер колонки
        /// </summary>
        /// <param name="index">Индекс колонки</param>
        public void AutoSizeColumn(uint index)
        {
            var addr = GetColumnAddr(index);
            var cells = Rows.SelectMany(x => x.r.Elements<Cell>().Where(z => GetAddrColumn(z.CellReference.Value) == addr));
            double maxw = 0;
            foreach (var c in cells)
            {
                if (c.StyleIndex == null) continue;
                if (stylesPart.Stylesheet.CellFormats.Elements().Count() < c.StyleIndex.Value) continue;

                var st = stylesPart.Stylesheet.CellFormats.ElementAt(Convert.ToInt32(c.StyleIndex.Value)) as CellFormat;
                if (st == null) continue;
                if (stylesPart.Stylesheet.Fonts.Elements().Count() < st.FontId.Value) continue;
                var font = stylesPart.Stylesheet.Fonts.ElementAt(Convert.ToInt32(st.FontId.Value)) as Font;
                if (font == null) continue;
                var text = "";
                if (c.CellValue != null)
                {
                    text = c.CellValue.Text;
                }
                if (c.InlineString != null)
                {
                    text = c.InlineString.Text.Text;
                }
                if (text == "")
                    continue;

                if (st.Alignment != null)
                {
                    if (st.Alignment.WrapText != null)
                    {
                        if (st.Alignment.WrapText == true)
                        {
                            text = text.Split(' ').Where(x => !string.IsNullOrEmpty(x)).Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur);
                        }
                    }
                }
                var w = GetWidth(font.FontName.Val.Value, font.FontSize.Val.Value, text);
                if (maxw < w)
                    maxw = w;


            }
            if (maxw != 0)
            {
                var t = GetColumn(addr);
                t.Col.Width = maxw;
                t.Col.CustomWidth = true;
            }

        }
        /// <summary>
        /// Авторазмер колонок
        /// </summary>
        /// <param name="from">с</param>
        /// <param name="to">по</param>
        public void AutoSizeColumns(uint from, uint to)
        {
            for (var i = from; i <= to; i++)
            {
                AutoSizeColumn(i);
            }
        }

        /// <summary>
        /// Создать стиль
        /// </summary>
        /// <param name="font">Шрифт</param>
        /// <param name="border">Граница</param>
        /// <param name="fill">Заливка</param>
        /// <returns></returns>
        public uint CreateType(FontOpenXML font, BorderOpenXML border, FillOpenXML fill)
        {

            uint fontid = 0;
            uint borderid = 0;
            uint fillid = 0;
            var al = new Alignment();
            uint formatid = 0;
            if (font != null)
            {
                var f = new Font();
                if (font.Bold) f.AppendChild(new Bold());
                if (font.Italic) f.AppendChild(new Italic());
                if (font.Underline) f.AppendChild(new Underline());
                f.AppendChild(new FontSize() { Val = font.size });
                f.AppendChild(new Color() { Rgb = new HexBinaryValue() { Value = font.color.ToString().Replace("#", "") } });
                f.AppendChild(new FontName() { Val = font.fontname });
                stylesPart.Stylesheet.Fonts.AppendChild(f);
                stylesPart.Stylesheet.Fonts.Count = Convert.ToUInt32(stylesPart.Stylesheet.Fonts.Count());
                fontid = stylesPart.Stylesheet.Fonts.Count - 1;
                al.Horizontal = font.HorizontalAlignment.ToHorizontalAlignmentValues();
                al.Vertical = font.VerticalAlignment.ToVerticalAlignmentValues();
                al.WrapText = font.wordwrap;
                formatid = font.Format;
            }

            if (fill != null)
            {
                stylesPart.Stylesheet.Fills.AppendChild(
                                new Fill(
                                    new PatternFill(
                                         new ForegroundColor
                                         {
                                             Rgb = new HexBinaryValue { Value = fill.color.ToString().Replace("#", "") }
                                         }
                                                   )
                                    { PatternType = PatternValues.Solid }
                                         )
                                                        );
                stylesPart.Stylesheet.Fills.Count = Convert.ToUInt32(stylesPart.Stylesheet.Fills.Count());
                fillid = stylesPart.Stylesheet.Fills.Count - 1;
            }


            if (border != null)
            {
                stylesPart.Stylesheet.Borders.AppendChild(
                     new Border(                                                         // Index 1 – Applies a Left, Right, Top, Bottom border to a cell
                        new LeftBorder(new Color() { Auto = true }) { Style = border.LeftBorder.ToBorderStyleValues() },
                        new RightBorder(new Color() { Auto = true }) { Style = border.RightBorder.ToBorderStyleValues() },
                        new TopBorder(new Color() { Auto = true }) { Style = border.TopBorder.ToBorderStyleValues() },
                        new BottomBorder(new Color() { Auto = true }) { Style = border.BottomBorder.ToBorderStyleValues() }));
                stylesPart.Stylesheet.Borders.Count = Convert.ToUInt32(stylesPart.Stylesheet.Borders.Count());
                borderid = stylesPart.Stylesheet.Borders.Count - 1;


            }


            stylesPart.Stylesheet.CellFormats.AppendChild(new CellFormat(al) { FontId = fontid, FillId = fillid, BorderId = borderid, NumberFormatId = formatid });
            stylesPart.Stylesheet.CellFormats.Count = Convert.ToUInt32(stylesPart.Stylesheet.CellFormats.Count());
            return stylesPart.Stylesheet.CellFormats.Count - 1;
        }
        /// <summary>
        /// Копировать строку
        /// </summary>
        /// <param name="From">Источник</param>
        /// <param name="To">Куда</param>
        /// <param name="Count">Кол-во копий</param>
        public void CopyRow(uint From, uint To, int Count = 1)
        {
            var rFrom = GetRow(From);
            //Находим все строки чтоб сдвинуть индекс
            var rows = Rows.Where(x => x.RowIndex >= To).ToList();
            MRow FirstRow = null;
            //Находим после чего вставлять
            if (rows.Any())
            {
                FirstRow = rows.FirstOrDefault();
                IncAdressRows(Count, rows.ToArray());
            }

            for (uint i = 0; i < Count; i++)
            {
                var rNew = GetCopyRow(rFrom, To + i);
                //вставляем
                AddMRow(rNew, FirstRow);
            }
        }


        /// <summary>
        /// Вставить строку
        /// </summary>
        /// <param name="To"></param>
        /// <param name="Count"></param>
        public void InsertRow(uint To, int Count)
        {
            //Находим все строки чтоб сдвинуть индекс
            var rows = Rows.Where(x => x.RowIndex >= To).OrderBy(x => x.RowIndex).ToList();
            MRow FirstRow = null;
            //Находим после чего вставлять
            if (rows.Any())
            {
                FirstRow = rows.FirstOrDefault();
                IncAdressRows(Count, rows.ToArray());
            }

            //вставляем
            for (var i = 0; i < Count; i++)
            {
                var rNew = new MRow(new Row { RowIndex = To });
                //вставляем
                AddMRow(rNew, FirstRow);
            }
        }

        private MRow GetCopyRow(MRow row, uint NEWINDEX)
        {
            var newrow = new MRow(new Row(row.r.OuterXml));
            newrow.r.RowIndex = NEWINDEX;
            foreach (var c in newrow.r.Elements<Cell>())
            {
                c.CellReference.Value = GetAddrColumn(c.CellReference.Value) + NEWINDEX;
            }
            //сдивигаем регионы строки
            foreach (var merge in row.MMergeCells)
            {
                var newMerge = new MMergeCell();
                newMerge.CRA = new CellRangeAddress(merge.CRA.From.Row, merge.CRA.From.Cell, merge.CRA.To.Row, merge.CRA.To.Cell, merge.CRA.List);
                newMerge.CRA.From.Row = newMerge.CRA.To.Row = NEWINDEX;
                newMerge.MergeCell = new MergeCell();
                newMerge.MergeCell.Reference = ToMergeRegion(newMerge.CRA);
                newrow.MMergeCells.Add(newMerge);

            }
            return newrow;
        }
        private void IncAdressRows(int INC, params MRow[] rows)
        {
            foreach (var row in rows)
            {
                rowsDictionary.Remove(row.r.RowIndex);
                var newIndex = row.r.RowIndex + INC;
                row.r.RowIndex = Convert.ToUInt32(newIndex);
                IncCell(row, INC);
                IncMerge(row, INC);
            }
            foreach (var row in rows)
            {
                rowsDictionary.Add(row.r.RowIndex, row);
            }
        }


        private void IncCell(MRow row, int INC)
        {
            //сдвигаем ячейки
            foreach (var c in row.r.Elements<Cell>())
            {
                c.CellReference.Value = GetAddrColumn(c.CellReference.Value) + (GetAddrRow(c.CellReference.Value) + INC);
            }
        }
        private void IncMerge(MRow row, int INC)
        {
            //сдвигаем ячейки
            foreach (var merge in row.MMergeCells)
            {
                var newIndexFrom = merge.CRA.From.Row + INC;
                var newIndexTo = merge.CRA.To.Row + INC;
                merge.CRA.From.Row = Convert.ToUInt32(newIndexFrom);
                merge.CRA.To.Row = Convert.ToUInt32(newIndexTo);
                merge.MergeCell.Reference = ToMergeRegion(merge.CRA);
            }
        }



        private string ToMergeRegion(CellRangeAddress addr)
        {
            return $"{GetColumnAddr(addr.From.Cell)}{addr.From.Row}:{GetColumnAddr(addr.To.Cell)}{addr.To.Row}";
        }
        private CellRangeAddress FromMergeRegion(string addr)
        {
            var parse_str = addr.Split(':');

            var cra = new CellRangeAddress(GetAddrRow(parse_str[0]), GetColumnIndex(parse_str[0]), GetAddrRow(parse_str[1]), GetColumnIndex(parse_str[1]));
            return cra;

        }
        /// <summary>
        /// Создать формат числа
        /// </summary>
        /// <param name="format">Формат</param>
        /// <returns></returns>
        public uint CreateNumFormatCustom(string format)
        {
            var fmt = new NumberingFormat() { FormatCode = format };
            if (stylesPart.Stylesheet.NumberingFormats == null)
                stylesPart.Stylesheet.NumberingFormats = new NumberingFormats();
            var curr = stylesPart.Stylesheet.NumberingFormats.Elements<NumberingFormat>().FirstOrDefault(x => x.FormatCode == fmt.FormatCode);
            var LAST = stylesPart.Stylesheet.NumberingFormats.Elements<NumberingFormat>().LastOrDefault();
            if (curr != null)
            {
                return curr.NumberFormatId;
            }

            fmt.NumberFormatId = LAST != null ? LAST.NumberFormatId : 165;
            stylesPart.Stylesheet.NumberingFormats.AppendChild(fmt);
            return fmt.NumberFormatId;
        }

        /// <summary>
        /// Подогнать размер строки ~
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Col">Ячейка</param>
        /// <param name="CountChar"> Кол-во символов на строке</param>
        /// <param name="SizeRow">Размер в точках 1 строки</param>
        public double Fit(uint Row, uint Col, int CountChar, int SizeRow)
        {
            var r = GetRow(Row);
            return Fit(r, Col, CountChar, SizeRow);
        }
        /// <summary>
        /// Подогнать размер строки ~
        /// </summary>
        /// <param name="r">Строка</param>
        /// <param name="Col">Ячейка</param>
        /// <param name="CountChar"> Кол-во символов на строке</param>
        /// <param name="SizeRow">Размер в точках 1 строки</param>
        public double Fit(MRow r, uint Col, int CountChar, int SizeRow)
        {
            var text = GetValue(r, Col);
            r.Height = SizeRow * Calc(text, CountChar);
            return r.Height;
        }
        public void SetRowHeigth(uint Row, double Heigth)
        {
            var r = GetRow(Row);
            r.Height = Heigth;
        }
        int Calc(string str, int countChar)
        {
            var split = (str ?? "").Split(' ');
            var sum = 0;
            var result = 1;
            foreach (var s in split)
            {
                sum += s.Length;
                if (sum > countChar)
                {
                    result++;
                    sum = s.Length;
                }
            }
            return result;
        }

        FontOpenXML GetFont(uint i)
        {
            var rs = new FontOpenXML();
            var f = Convert.ToInt32((stylesPart.Stylesheet.CellFormats.ChildElements[Convert.ToInt32(i)] as CellFormat).FontId.Value);
            rs.Bold = (stylesPart.Stylesheet.Fonts.ChildElements[f] as Font).GetFirstChild<Bold>().Val;
            rs.Italic = (stylesPart.Stylesheet.Fonts.ChildElements[f] as Font).GetFirstChild<Italic>().Val;
            rs.fontname = (stylesPart.Stylesheet.Fonts.ChildElements[f] as Font).GetFirstChild<FontName>().Val;
            rs.size = (stylesPart.Stylesheet.Fonts.ChildElements[f] as Font).GetFirstChild<FontSize>().Val;
            return rs;
        }
        private Stylesheet GenerateStyleSheet()
        {
            return new Stylesheet(
                new Fonts(
                    new Font(                                                               // Index 0 – The default font.
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "00000000" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(                                                               // Index 1 – The bold font.
                        new Bold(),
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "00000000" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(                                                               // Index 2 – The Italic font.
                        new Italic(),
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "00000000" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(                                                               // Index 2 – The Times Roman font. with 16 size
                        new FontSize() { Val = 16 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "00000000" } },
                        new FontName() { Val = "Times New Roman" })
                ),
                new Fills(
                    new Fill(                                                           // Index 0 – The default fill.
                        new PatternFill() { PatternType = PatternValues.None }),
                    new Fill(                                                           // Index 1 – The default fill of gray 125 (required)
                        new PatternFill() { PatternType = PatternValues.Gray125 }),
                    new Fill(                                                           // Index 2 – The yellow fill.
                        new PatternFill(
                            new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFFFFF00" } }
                        )
                        { PatternType = PatternValues.Solid })
                ),
                new Borders(
                    new Border(                                                         // Index 0 – The default border.
                        new LeftBorder(),
                        new RightBorder(),
                        new TopBorder(),
                        new BottomBorder(),
                        new DiagonalBorder()),
                    new Border(                                                         // Index 1 – Applies a Left, Right, Top, Bottom border to a cell
                        new LeftBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new RightBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new TopBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new BottomBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                ),
                new CellFormats(
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 },                          // Index 0 – The default cell style.  If a cell does not have a style index applied it will use this style combination instead
                    new CellFormat() { FontId = 1, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 1 – Bold 
                    new CellFormat() { FontId = 2, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 2 – Italic
                    new CellFormat() { FontId = 3, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 3 – Times Roman
                    new CellFormat() { FontId = 0, FillId = 2, BorderId = 0, ApplyFill = true },       // Index 4 – Yellow Fill
                    new CellFormat(                                                                   // Index 5 – Alignment
                        new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                    )
                    { FontId = 0, FillId = 0, BorderId = 0, ApplyAlignment = true },
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }      // Index 6 – Border
                )
            ); // return
        }
        private Stylesheet GenerateDefaultStyleSheet()
        {
            return new Stylesheet(
                new Fonts(
                    new Font(
                        // Index 0 – The default font.
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "00000000" } },
                        new FontName() { Val = "Calibri" }))
                ,
                new Fills(
                    new Fill(                                                           // Index 0 – The default fill.
                        new PatternFill() { PatternType = PatternValues.None }),
                    new Fill(new PatternFill() { PatternType = PatternValues.Gray125 })
                        )
                ,
                new Borders(
                    new Border(                                                         // Index 0 – The default border.
                        new LeftBorder(),
                        new RightBorder(),
                        new TopBorder(),
                        new BottomBorder(),
                        new DiagonalBorder())
                ),
                new CellFormats(
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 }                       // Index 0 – The default cell style.  If a cell does not have a style index applied it will use this style combination instead

                )
            ); // return
        }
        /// <summary>
        /// Установить текущий лист
        /// </summary>
        /// <param name="index">индекс (от 0 до N)</param>
        public void SetCurrentSchet(int index)
        {
            var sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
            worksheetPart = (WorksheetPart)workbookPart.GetPartById(((Sheet)sheets.ChildElements[index]).Id);
            sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
            //Колонки
            // cols = worksheetPart.Worksheet.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Columns>() ?? new DocumentFormat.OpenXml.Spreadsheet.Columns();
            ClearParam();
            ReadParam();
            //Таблица стилей
            stylesPart = workbookPart.GetPartsOfType<WorkbookStylesPart>().First();// AddNewPart<WorkbookStylesPart>();
            if (stylesPart.Stylesheet == null)
                stylesPart.Stylesheet = GenerateDefaultStyleSheet();
        }
        /// <summary>
        /// Установить текущий лист
        /// </summary>
        /// <param name="name">Имя листа</param>
        public void SetCurrentSchet(string name)
        {
            var sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
            worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheets.Elements<Sheet>().First(x => x.Name == name).Id);
            sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
            //Колонки
            // cols = worksheetPart.Worksheet.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Columns>() ?? new DocumentFormat.OpenXml.Spreadsheet.Columns();
            ClearParam();
            ReadParam();
            //Таблица стилей
            stylesPart = workbookPart.GetPartsOfType<WorkbookStylesPart>().First();// AddNewPart<WorkbookStylesPart>();
            if (stylesPart.Stylesheet == null)
                stylesPart.Stylesheet = GenerateDefaultStyleSheet();
        }

        /// <summary>
        /// Получить строку
        /// </summary>
        /// <param name="rowIndex">Индекс</param>
        /// <param name="news">Флаг новой строки, если True то поиск существуещей строки производится не будет. По умолчанию False</param>
        /// <returns></returns>
        public MRow GetRow(uint rowIndex, bool news = false)
        {
            if (!rowsDictionary.ContainsKey(rowIndex))
            {
                var mrow = new MRow(new Row { RowIndex = rowIndex });
                sheetData.AppendChild(mrow.r);
                rowsDictionary.Add(rowIndex, mrow);
            }
            return rowsDictionary[rowIndex];
        }
        /// <summary>
        /// Удалить строку
        /// </summary>
        /// <param name="rowIndex"></param>
        public void RemoveRow(uint rowIndex)
        {
            DeleteRow(GetRow(rowIndex));

            //Находив все строки чтоб сдвинуть индекс
            var rows = Rows.Where(r => r.RowIndex > rowIndex).ToList();
            //Находим после чего вставлять
            if (rows.Any())
            {
                IncAdressRows(-1, rows.ToArray());
            }
        }

        private void DeleteRow(MRow row)
        {
            row.r.Remove();
            rowsDictionary.Remove(row.RowIndex);
            row.MMergeCells.ForEach(x =>
            {
                x.MergeCell.Remove();
                MergeCells?.Remove(x);
            });
        }
        // Given an Excel address such as E5 or AB128, GetRowIndex
        // parses the address and returns the row index.
        private uint GetRowIndex(string address)
        {
            uint result = 0;

            for (var i = 0; i < address.Length; i++)
            {
                uint l;
                if (uint.TryParse(address.Substring(i, 1), out l))
                {
                    var rowPart = address.Substring(i, address.Length - i);
                    if (uint.TryParse(rowPart, out l))
                    {
                        result = l;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Получить имя колонки по индексу
        /// </summary>
        /// <param name="colNum">Индекс</param>
        /// <returns></returns>
        public static string GetColumnAddr(uint colNum)
        {
            return GetExcelColumnName(colNum);
        }

        private static string GetExcelColumnName(uint columnNumber)
        {
            var dividend = columnNumber;
            var columnName = string.Empty;
            uint modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return columnName;
        }
        private uint GetColumnIndex(string address)
        {
            address = GetAddrColumn(address);
            uint res = 0;
            foreach (var c in address)
            {
                res = res * 26;
                res += (uint)c - 64;
            }
            return res;
        }


        /// <summary>
        /// Получить колонку из адреса
        /// </summary>
        /// <param name="addr">Адрес (например ACF12)</param>
        /// <returns>Колонка (например ACF)</returns>
        private string GetAddrColumn(string addr)
        {
            var res = string.Empty;
            foreach (var c in addr)
            {
                if (char.IsLetter(c))
                    res += c;
            }
            return res.ToUpper();
        }

        private uint GetAddrRow(string addr)
        {
            var res = string.Empty;
            foreach (var c in addr)
            {
                if (char.IsDigit(c))
                    res += c;
            }
            return Convert.ToUInt32(res);
        }

        // Given a Worksheet and an address (like "AZ254"), either return a 
        // cell reference, or create the cell reference and return it.
        private Cell InsertCellInWorksheet(Row row, string addressName)
        {
            Cell cell = null;
            // If the cell you need already exists, return it.
            // If there is not a cell with the specified column name, insert one.  
            var refCell = row.Elements<Cell>().FirstOrDefault(c => c.CellReference.Value == addressName);
            if (refCell != null)
            {
                cell = refCell;
            }
            else
            {
                cell = CreateCell(row, addressName);
            }
            return cell;
        }


        // Add a cell with the specified address to a row.
        private Cell CreateCell(Row row, string address)
        {
            var addr = GetColumnIndex(address);
            var refCell = row.Elements<Cell>().FirstOrDefault(cell => GetColumnIndex(cell.CellReference.Value) > addr);
            // Cells must be in sequential order according to CellReference. 
            // Determine where to insert the new cell.
            var cellResult = new Cell();
            cellResult.CellReference = address;
            row.InsertBefore(cellResult, refCell);
            return cellResult;
        }

        /// <summary>
        /// Получить значение ячейки
        /// </summary>
        /// <param name="row">Строка</param>
        /// <param name="index">Ячейка</param>
        /// <returns></returns>
        public string GetValue(MRow row, uint index)
        {
            var addr = GetColumnAddr(index) + row.r.RowIndex.ToString();
            var value = "";
            foreach (var cell in row.r.Elements<Cell>())
            {
                if (cell.CellReference.Value == addr)
                {

                    if (cell.DataType != null)
                    {
                        switch (cell.DataType.Value)
                        {
                            case CellValues.SharedString:
                                var stringTable = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                                if (stringTable != null)
                                {
                                    value = stringTable.SharedStringTable.ElementAt(int.Parse(cell.CellValue.Text)).InnerText;
                                }
                                break;
                            case CellValues.InlineString:
                                value = cell.InlineString.Text.Text;
                                break;
                            default:
                                if (cell.CellValue == null) return value = "";
                                value = cell.CellValue.Text;
                                break;

                        }
                    }
                    else
                    {

                        value = cell.CellValue?.Text;
                    }

                    return value;
                }
            }
            return value;
        }

        /// <summary>
        /// Получить строки
        /// </summary>
        public IEnumerable<MRow> Rows => rowsDictionary.Values.OrderBy(x => x.RowIndex);
        /*
        /// <summary>
        /// Просчитать кол-во страниц
        /// </summary>
        /// <param name="App">Приложение</param>
        /// <param name="path">Путь</param>
        /// <param name="sheet">Лист</param>
        /// <returns></returns>
        public static int GetPageCount(Microsoft.Office.Interop.Excel.Application App, string path, int sheet)
        {
            try
            {
                var workbooks = App.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook book = null;
                book = workbooks.Open(path, 1, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", false, false, 0, true, false, Microsoft.Office.Interop.Excel.XlCorruptLoad.xlNormalLoad);

                var Curr = (Microsoft.Office.Interop.Excel.Worksheet)book.Sheets[sheet];
                var Count = Curr.HPageBreaks.Count + 1;
                book.Close(SaveChanges: false);
                workbooks.Close();
                return Count;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при считывании кол-ва страниц", ex);
            }

        }
        /// <summary>
        /// Получить приложение EXCEL
        /// </summary>
        /// <returns></returns>
        public static Microsoft.Office.Interop.Excel.Application GetAppExcel()
        {
            return new Microsoft.Office.Interop.Excel.Application();



        }

        /// <summary>
        /// Закрыть приложение Excel
        /// </summary>
        /// <param name="app"></param>
        public static void KillAppExcel(ref Microsoft.Office.Interop.Excel.Application app)
        {
            app.Quit();
            app = null;
            GC.Collect();
        }*/
    }

    /// <summary>
    /// Диапазон адресов
    /// </summary>
    public class CellRangeAddress
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="rowFrom"></param>
        /// <param name="cellFrom"></param>
        /// <param name="rowTo"></param>
        /// <param name="cellTo"></param>
        /// <param name="list"></param>
        public CellRangeAddress(uint rowFrom, uint cellFrom, uint rowTo, uint cellTo, string list = "")
        {
            From = new CellAddress(rowFrom, cellFrom);
            To = new CellAddress(rowTo, cellTo);
            List = list;
        }
        /// <summary>
        /// 
        /// </summary>
        public CellAddress From { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CellAddress To { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string List { get; set; }
        /// <summary>
        /// Регион в одной строке
        /// </summary>
        /// public bool IsOnewRows => From.Row == To.Row;
    }
    /// <summary>
    /// Адрес ячейки
    /// </summary>
    public class CellAddress
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="row"></param>
        /// <param name="cell"></param>
        public CellAddress(uint row, uint cell)
        {
            Row = row;
            Cell = cell;
        }
        /// <summary>
        /// 
        /// </summary>
        public uint Row { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public uint Cell { get; set; }
    }

    /// <summary>
    /// Создание файла Excel в поток
    /// </summary>
    public class ExcelOpenXMLSAX
    {
        //Sheet sheet;
        WorkbookPart workbookPart;
        WorksheetPart worksheetPart;
        // WorksheetPart worksheetPart;
        SpreadsheetDocument document;
        WorkbookStylesPart stylesPart;
        /// <summary>
        /// Колонки
        /// </summary>
        public Dictionary<string, ColumnOpenXML> Columns { get; set; }
        Columns cols;
        OpenXmlWriter wr;
        Stream stream;
        /// <summary>
        /// Создать файл
        /// </summary>
        /// <param name="fileName">Путь</param>
        /// <param name="name_sheet">Имя листа</param>
        public ExcelOpenXMLSAX(string fileName, string name_sheet)
        {
            this.AutoFit = false;
            //Создаем документ
            document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook);
            // Создаем книгу
            workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();
            // Создаем лист
            worksheetPart = workbookPart.AddNewPart<WorksheetPart>();




            //Список листов
            var sheets = workbookPart.Workbook.AppendChild(new Sheets());
            //Лист
            var sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = name_sheet };
            sheets.Append(sheet);
            //Ячейки листаwr = OpenXmlWriter.Create(worksheetPart);
            stream = worksheetPart.GetStream();
            wr = OpenXmlWriter.Create(stream);
            wr.WriteStartElement(new Worksheet());


            //Колонки
            cols = new DocumentFormat.OpenXml.Spreadsheet.Columns();


            //Добавляем колонки и ячейки
            //   worksheetPart.Worksheet.Append(cols);


            //Таблица стилей
            stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
            stylesPart.Stylesheet = GenerateDefaultStyleSheet();
            Columns = new Dictionary<string, ColumnOpenXML>();


        }

        /// <summary>
        /// Вставить колнку
        /// </summary>
        public void WriteColumns()
        {
            wr.WriteElement(cols);
            wr.WriteStartElement(new SheetData());
        }


        /// <summary>
        /// Добавить лист
        /// </summary>
        /// <param name="name_sheet"></param>
        public void AddSchet(string name_sheet)
        {
            // Лист
            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();

            //Находим список листов
            var sheets = workbookPart.Workbook.GetFirstChild<Sheets>();
            //Указатель на лист
            var relationshipId = workbookPart.GetIdOfPart(worksheetPart);
            // Получаем уникальный ID
            uint sheetId = 1;
            if (sheets.Elements<Sheet>().Count() > 0)
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }
            // Новый лист
            var sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = name_sheet };
            sheets.Append(sheet);

            //Колонки + данные о ячейках
            // cols = new DocumentFormat.OpenXml.Spreadsheet.Columns();
            // worksheetPart.Worksheet.Append(cols);
            if (wr != null)
                CloseWr();

            wr = OpenXmlWriter.Create(worksheetPart);
            wr.WriteStartElement(new Worksheet());
            Columns = new Dictionary<string, ColumnOpenXML>();
        }

        /// <summary>
        /// Получить колонку
        /// </summary>
        /// <param name="Addr">Адрес</param>
        /// <param name="width">Ширина</param>
        /// <returns></returns>

        public ColumnOpenXML GetColumn(string Addr, double width)
        {
            if (Columns.ContainsKey(Addr))
            {
                return Columns[Addr];
            }

            var c = new Column();
            c.Min = c.Max = GetColumnIndex(Addr);
            c.Width = DoubleValue.FromDouble(width);
            cols.Append(c);
            var copx = new ColumnOpenXML() { Col = c };
            Columns.Add(Addr, copx);
            return copx;
        }


        private void CloseWr()
        {
            wr.WriteEndElement();
            wr.WriteEndElement();
            wr.Close();
        }
        /// <summary>
        /// Сохранить
        /// </summary>
        public void Save()
        {
            CloseWr();
            if (document.WorkbookPart.Workbook.CalculationProperties == null)
            {
                document.WorkbookPart.Workbook.CalculationProperties = new CalculationProperties();
            }
            document.WorkbookPart.Workbook.CalculationProperties.ForceFullCalculation = true;
            document.WorkbookPart.Workbook.CalculationProperties.FullCalculationOnLoad = true;
            document.Close();
            document.Dispose();
        }
        /// <summary>
        /// Включить автоподбор размера при внесении значения в колонку
        /// </summary>
        public bool AutoFit { get; set; }
        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Ячейка</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>

        public void PrintCell(MRow Row, string Cell, string value, uint? styleid)
        {
            //  var Row = GetRow(RowI);
            var cell = CreateCell(Row.r, Cell + Row.r.RowIndex);
            cell.DataType = CellValues.InlineString;
            var istring = new InlineString { Text = new Text { Text = value } };
            cell.InlineString = istring;
            if (styleid.HasValue)
                cell.StyleIndex = styleid.Value;

            WriteCell(cell);
        }
        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Ячейка</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>
        public void PrintCell(MRow Row, string Cell, DateTime? value, uint? styleid)
        {
            //  var Row = GetRow(RowI);
            var cell = CreateCell(Row.r, Cell + Row.r.RowIndex.ToString());
            // cell.DataType = CellValues.Date;
            cell.CellValue = value.HasValue ? new CellValue { Text = value.Value.ToOADate().ToString() } : new CellValue { Text = "" };
            if (styleid.HasValue)
                cell.StyleIndex = styleid.Value;

            WriteCell(cell);

        }
        /// <summary>
        /// Вставить значение
        /// </summary>
        /// <param name="Row">Строка</param>
        /// <param name="Cell">Ячейка</param>
        /// <param name="value">Значение</param>
        /// <param name="styleid">Стиль</param>
        public void PrintCell(MRow Row, string Cell, double value, uint? styleid)
        {

            var cell = CreateCell(Row.r, Cell + Row.r.RowIndex.ToString());

            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
            if (styleid.HasValue)
                cell.StyleIndex = styleid.Value;
            cell.CellValue = new CellValue() { Text = value.ToString().Replace(",", ".") };
            WriteCell(cell);
        }





        /// <summary>
        /// Создать стиль
        /// </summary>
        /// <param name="font">Шрифт</param>
        /// <param name="border">Грани</param>
        /// <param name="fill">Заливка</param>
        /// <returns></returns>
        public uint CreateType(FontOpenXML font, BorderOpenXML border, FillOpenXML fill)
        {

            uint fontid = 0;
            uint borderid = 0;
            uint fillid = 0;
            if (font != null)
            {
                stylesPart.Stylesheet.Fonts.AppendChild(
                            new Font(
                                new Bold() { Val = font.Bold },
                                new Italic() { Val = font.Italic },
                                new FontSize() { Val = font.size },
                                new Color() { Rgb = new HexBinaryValue() { Value = font.color.ToString().Replace("#", "") } },
                                new FontName() { Val = font.fontname }
                                    )
                                                        );
                fontid = Convert.ToUInt32(stylesPart.Stylesheet.Fonts.Count() - 1);

            }

            if (fill != null)
            {
                stylesPart.Stylesheet.Fills.AppendChild(
                                new Fill(
                                    new PatternFill(
                                         new ForegroundColor()
                                         {
                                             Rgb = new HexBinaryValue() { Value = fill.color.ToString().Replace("#", "") }
                                         }
                                                   )
                                    { PatternType = PatternValues.Solid }
                                         )
                                                        );
                fillid = Convert.ToUInt32(stylesPart.Stylesheet.Fills.Count() - 1);
            }


            if (border != null)
            {
                stylesPart.Stylesheet.Borders.AppendChild(
                     new Border(                                                         // Index 1 – Applies a Left, Right, Top, Bottom border to a cell
                        new LeftBorder(new Color() { Auto = true }) { Style = border.LeftBorder.ToBorderStyleValues() },
                        new RightBorder(new Color() { Auto = true }) { Style = border.RightBorder.ToBorderStyleValues() },
                        new TopBorder(new Color() { Auto = true }) { Style = border.TopBorder.ToBorderStyleValues() },
                        new BottomBorder(new Color() { Auto = true }) { Style = border.BottomBorder.ToBorderStyleValues() }));

                borderid = Convert.ToUInt32(stylesPart.Stylesheet.Borders.Count() - 1);
            }
            var al = new Alignment();
            if (font != null)
            {
                al.Horizontal = font.HorizontalAlignment.ToHorizontalAlignmentValues();
                al.Vertical = font.VerticalAlignment.ToVerticalAlignmentValues();
                al.WrapText = font.wordwrap;
            }
            uint formatid = 0;
            if (font != null)
                formatid = font.Format;

            stylesPart.Stylesheet.CellFormats.AppendChild(
              new CellFormat(al) { FontId = fontid, FillId = fillid, BorderId = borderid, NumberFormatId = formatid }      // Index 6 – Border
            );

            return Convert.ToUInt32(stylesPart.Stylesheet.CellFormats.Count() - 1);
        }




        FontOpenXML GetFont(uint i)
        {
            var rs = new FontOpenXML();
            var f = Convert.ToInt32((stylesPart.Stylesheet.CellFormats.ChildElements[Convert.ToInt32(i)] as CellFormat).FontId.Value);
            rs.Bold = (stylesPart.Stylesheet.Fonts.ChildElements[f] as Font).GetFirstChild<Bold>().Val;
            rs.Italic = (stylesPart.Stylesheet.Fonts.ChildElements[f] as Font).GetFirstChild<Italic>().Val;
            rs.fontname = (stylesPart.Stylesheet.Fonts.ChildElements[f] as Font).GetFirstChild<FontName>().Val;
            rs.size = (stylesPart.Stylesheet.Fonts.ChildElements[f] as Font).GetFirstChild<FontSize>().Val;
            return rs;
        }



        private Stylesheet GenerateStyleSheet()
        {
            return new Stylesheet(
                new Fonts(
                    new Font(                                                               // Index 0 – The default font.
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(                                                               // Index 1 – The bold font.
                        new Bold(),
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(                                                               // Index 2 – The Italic font.
                        new Italic(),
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Calibri" }),
                    new Font(                                                               // Index 2 – The Times Roman font. with 16 size
                        new FontSize() { Val = 16 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Times New Roman" })
                ),
                new Fills(
                    new Fill(                                                           // Index 0 – The default fill.
                        new PatternFill() { PatternType = PatternValues.None }),
                    new Fill(                                                           // Index 1 – The default fill of gray 125 (required)
                        new PatternFill() { PatternType = PatternValues.Gray125 }),
                    new Fill(                                                           // Index 2 – The yellow fill.
                        new PatternFill(
                            new ForegroundColor() { Rgb = new HexBinaryValue() { Value = "FFFFFF00" } }
                        )
                        { PatternType = PatternValues.Solid })
                ),
                new Borders(
                    new Border(                                                         // Index 0 – The default border.
                        new LeftBorder(),
                        new RightBorder(),
                        new TopBorder(),
                        new BottomBorder(),
                        new DiagonalBorder()),
                    new Border(                                                         // Index 1 – Applies a Left, Right, Top, Bottom border to a cell
                        new LeftBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new RightBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new TopBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new BottomBorder(
                            new Color() { Auto = true }
                        )
                        { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                ),
                new CellFormats(
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 },                          // Index 0 – The default cell style.  If a cell does not have a style index applied it will use this style combination instead
                    new CellFormat() { FontId = 1, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 1 – Bold 
                    new CellFormat() { FontId = 2, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 2 – Italic
                    new CellFormat() { FontId = 3, FillId = 0, BorderId = 0, ApplyFont = true },       // Index 3 – Times Roman
                    new CellFormat() { FontId = 0, FillId = 2, BorderId = 0, ApplyFill = true },       // Index 4 – Yellow Fill
                    new CellFormat(                                                                   // Index 5 – Alignment
                        new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center }
                    )
                    { FontId = 0, FillId = 0, BorderId = 0, ApplyAlignment = true },
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }      // Index 6 – Border
                )
            ); // return
        }


        private Stylesheet GenerateDefaultStyleSheet()
        {
            return new Stylesheet(
                new Fonts(
                    new Font(                                                               // Index 0 – The default font.
                        new FontSize() { Val = 11 },
                        new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                        new FontName() { Val = "Calibri" }))
                ,
                new Fills(
                    new Fill(                                                           // Index 0 – The default fill.
                        new PatternFill() { PatternType = PatternValues.None }),
                    new Fill(new PatternFill() { PatternType = PatternValues.Gray125 })
                        )
                ,
                new Borders(
                    new Border(                                                         // Index 0 – The default border.
                        new LeftBorder(),
                        new RightBorder(),
                        new TopBorder(),
                        new BottomBorder(),
                        new DiagonalBorder())
                ),
                new CellFormats(
                    new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 }                       // Index 0 – The default cell style.  If a cell does not have a style index applied it will use this style combination instead

                )
            ); // return
        }



        /// <summary>
        /// Получить строку
        /// </summary>
        /// <param name="rowIndex">Индекс</param>
        /// <returns></returns>
        public MRow GetRow(uint rowIndex)
        {

            Row row = null;
            row = new Row();
            row.RowIndex = rowIndex;
            return new MRow(row);
        }
        /// <summary>
        /// Начать строку
        /// </summary>
        /// <param name="row"></param>
        public void StartRow(MRow row)
        {
            wr.WriteStartElement(row.r);
        }
        /// <summary>
        /// Завершить строку
        /// </summary>
        public void EndRow()
        {
            wr.WriteEndElement();
        }

        private void WriteCell(OpenXmlElement cell)
        {
            wr.WriteElement(cell);
        }

        // Given an Excel address such as E5 or AB128, GetRowIndex
        // parses the address and returns the row index.
        private uint GetRowIndex(string address)
        {
            string rowPart;
            uint l;
            uint result = 0;

            for (var i = 0; i < address.Length; i++)
            {
                if (uint.TryParse(address.Substring(i, 1), out l))
                {
                    rowPart = address.Substring(i, address.Length - i);
                    if (uint.TryParse(rowPart, out l))
                    {
                        result = l;
                        break;
                    }
                }
            }
            return result;
        }
        private uint GetColumnIndex(string address)
        {
            address = address.ToUpper();
            uint res = 0;
            foreach (var c in address)
            {
                res += c - (uint)'A' + 1;
            }
            return res;
        }
        // Add a cell with the specified address to a row.
        private Cell CreateCell(Row row, string address)
        {
            var cellResult = new Cell { CellReference = address };
            return cellResult;
        }
    }
    /// <summary>
    /// Расширения
    /// </summary>
    public static class Ext
    {
        /// <summary>
        /// Конвертировать HorizontalAlignmentV в HorizontalAlignmentValues
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static HorizontalAlignmentValues ToHorizontalAlignmentValues(this HorizontalAlignmentV val)
        {
            switch (val)
            {
                case HorizontalAlignmentV.Center: return HorizontalAlignmentValues.Center;
                case HorizontalAlignmentV.Left: return HorizontalAlignmentValues.Left;
                default:
                    return HorizontalAlignmentValues.Right;
            }
        }
        /// <summary>
        /// Конвертировать VerticalAlignmentV в VerticalAlignmentValues
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static VerticalAlignmentValues ToVerticalAlignmentValues(this VerticalAlignmentV val)
        {
            switch (val)
            {
                case VerticalAlignmentV.Bottom: return VerticalAlignmentValues.Bottom;
                case VerticalAlignmentV.Center: return VerticalAlignmentValues.Center;
                default:
                    return VerticalAlignmentValues.Top;
            }
        }

        /// <summary>
        /// Конвертировать BorderValues в BorderStyleValues
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static BorderStyleValues ToBorderStyleValues(this BorderValues val)
        {
            switch (val)
            {
                case BorderValues.Thin: return DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.Thin;
                case BorderValues.None: return DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.None;
                default:
                    return DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.None;
            }
        }




    }
}
