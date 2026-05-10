# PDF转Word工具类 Spec

## Why
需要一个通用的工具类,将PDF文件转换为可编辑的Word文档,以便用户能够编辑和修改PDF中的内容。PDF格式虽然稳定但难以编辑,Word格式更适合内容修改和协作。

## What Changes
- 新增PDF转Word转换工具类
- 支持多种转换方案(商业库和开源库)
- 提供统一的转换接口
- 支持批量转换和单文件转换
- 支持转换进度回调

## Impact
- Affected specs: 新增文档转换能力
- Affected code: CommonTools项目将新增PdfConverter相关类

## ADDED Requirements

### Requirement: PDF转Word转换功能
系统应提供PDF到Word文档的转换功能,支持多种转换方案。

#### Scenario: 单文件转换成功
- **WHEN** 用户提供一个有效的PDF文件路径
- **THEN** 系统将其转换为Word文档并保存到指定位置

#### Scenario: 批量转换成功
- **WHEN** 用户提供多个PDF文件路径
- **THEN** 系统批量转换所有文件并返回转换结果列表

#### Scenario: 转换失败处理
- **WHEN** PDF文件损坏或格式不支持
- **THEN** 系统返回明确的错误信息,不抛出未处理异常

## 待选方案对比

### 方案1: Spire.PDF (推荐)

**库信息:**
- NuGet包: `Spire.PDF`
- 许可: 商业许可(免费版有页数限制)
- 官网: https://www.e-iceblue.com/

**优点:**
- ✅ 转换质量高,格式还原度好
- ✅ API简单易用,代码量少
- ✅ 支持固定布局和流式布局两种转换模式
- ✅ 免费版可试用(每文档最多10页)
- ✅ 中文支持良好
- ✅ 持续更新维护

**缺点:**
- ❌ 免费版有页数限制(每文档最多10页)
- ❌ 商业使用需要购买许可证(约$599起)
- ❌ 转换速度中等

**代码示例:**
```csharp
using Spire.Pdf;

public void ConvertPdfToWord(string pdfPath, string wordPath)
{
    PdfDocument doc = new PdfDocument();
    doc.LoadFromFile(pdfPath);
    doc.SaveToFile(wordPath, FileFormat.DOCX);
    doc.Close();
}
```

**适用场景:**
- 个人项目或小型企业
- 对转换质量有较高要求
- 预算允许购买商业许可

---

### 方案2: Aspose.PDF

**库信息:**
- NuGet包: `Aspose.PDF`
- 许可: 商业许可
- 官网: https://products.aspose.com/pdf/net

**优点:**
- ✅ 功能最强大,转换质量最高
- ✅ 支持复杂布局和格式
- ✅ 格式还原度极高,几乎完美还原
- ✅ 支持多种输出格式(DOC, DOCX, RTF等)
- ✅ 企业级技术支持

**缺点:**
- ❌ 价格昂贵(约$999起)
- ❌ 试用版有限制(最多4页+水印)
- ❌ 依赖系统字体库,缺少字体会导致乱码

**代码示例:**
```csharp
using Aspose.Pdf;

public void ConvertPdfToWord(string pdfPath, string wordPath)
{
    Document doc = new Document(pdfPath);
    DocSaveOptions saveOptions = new DocSaveOptions();
    saveOptions.Format = DocSaveOptions.DocFormat.DocX;
    doc.Save(wordPath, saveOptions);
}
```

**适用场景:**
- 大型企业项目
- 对转换质量要求极高
- 预算充足

---

### 方案3: iText7 (开源方案)

**库信息:**
- NuGet包: `itext7`
- 许可: AGPL(开源) / 商业许可
- 官网: https://itextpdf.com/

**优点:**
- ✅ 开源版本免费(AGPL许可)
- ✅ 社区活跃,文档丰富
- ✅ 功能全面,可自定义转换逻辑

**缺点:**
- ❌ **不支持直接PDF转Word**(需要自己实现转换逻辑)
- ❌ 需要大量开发工作
- ❌ 转换质量取决于实现
- ❌ AGPL许可证限制商业使用

**代码示例:**
```csharp
// iText7本身不直接支持PDF转Word
// 需要提取PDF内容,然后使用其他库生成Word
// 实现复杂度高
```

**适用场景:**
- 开源项目
- 有充足开发时间
- 需要高度自定义转换逻辑

---

### 方案4: SautinSoft.PdfFocus

**库信息:**
- NuGet包: `SautinSoft.PdfFocus`
- 许可: 商业许可
- 官网: https://www.sautinsoft.com/

**优点:**
- ✅ 专门做PDF转Word,专注度高
- ✅ 转换质量中等偏上
- ✅ 价格相对便宜(约$339)
- ✅ 支持批量转换

**缺点:**
- ❌ 商业许可需要付费
- ❌ 社区相对较小
- ❌ 文档不如前两者丰富

**代码示例:**
```csharp
using SautinSoft;

public void ConvertPdfToWord(string pdfPath, string wordPath)
{
    PdfFocus f = new PdfFocus();
    f.OpenPdf(pdfPath);
    f.ToWord(wordPath);
}
```

**适用场景:**
- 中小型企业
- 预算有限但需要商业许可
- 专注PDF转Word场景

---

### 方案5: Microsoft.Office.Interop.Word

**库信息:**
- NuGet包: 无(系统自带)
- 许可: 需要Office许可证
- 官网: Microsoft官方

**优点:**
- ✅ 免费(如果已有Office许可证)
- ✅ 与Office集成度高
- ✅ 转换质量取决于本地Office版本

**缺点:**
- ❌ **需要安装Microsoft Word**
- ❌ 依赖COM组件,不适合服务器环境
- ❌ 性能较差
- ❌ 不适合自动化批量处理
- ❌ 需要Windows环境

**代码示例:**
```csharp
using Microsoft.Office.Interop.Word;

public void ConvertPdfToWord(string pdfPath, string wordPath)
{
    Application wordApp = new Application();
    Document doc = wordApp.Documents.Open(pdfPath);
    doc.SaveAs2(wordPath, WdSaveFormat.wdFormatXMLDocument);
    doc.Close();
    wordApp.Quit();
}
```

**适用场景:**
- Windows桌面应用
- 已有Office许可证
- 非服务器环境

---

## 推荐方案

### 首选: Spire.PDF
**理由:**
1. 转换质量高,格式还原度好
2. API简单易用,开发成本低
3. 免费版可以试用和测试
4. 中文支持良好
5. 价格适中,性价比高

### 备选: Aspose.PDF
**理由:**
- 如果预算充足且对转换质量要求极高,可选择Aspose.PDF

### 不推荐: iText7
**理由:**
- 不直接支持PDF转Word,需要大量开发工作

## 技术选型建议

根据不同场景推荐:

| 场景 | 推荐方案 | 理由 |
|------|---------|------|
| 个人项目/学习 | Spire.PDF免费版 | 免费且功能够用 |
| 小型企业项目 | Spire.PDF付费版 | 性价比高,质量好 |
| 大型企业项目 | Aspose.PDF | 质量最高,企业支持 |
| 开源项目 | 不推荐PDF转Word | 开源方案不成熟 |
| Windows桌面应用 | Microsoft.Office.Interop | 免费(已有Office) |

## 实现建议

建议采用**策略模式**,支持多种转换方案的可插拔实现:

```csharp
public interface IPdfToWordConverter
{
    Task<ConversionResult> ConvertAsync(string pdfPath, string wordPath);
    Task<BatchConversionResult> ConvertBatchAsync(IEnumerable<string> pdfPaths, string outputFolder);
}

public class SpirePdfConverter : IPdfToWordConverter { }
public class AsposePdfConverter : IPdfToWordConverter { }
```

这样可以根据项目需求灵活切换转换引擎。
