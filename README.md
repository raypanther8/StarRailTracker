# StarRail Warp Tracker

`StarRail Warp Tracker` 是一個專為《崩壞：星穹鐵道》玩家設計的 C# 主控台（Console）工具，能幫你輕鬆紀錄、備份與分析遊戲中的躍遷（抽卡）歷史紀錄。

---

## ✨ 主要功能 (Features)

* 🔄 **自動抓取紀錄**：自動擷取《崩壞：星穹鐵道》最新的遊戲內躍遷歷史。
* 📥 **JSON 資料匯入**：支援匯入先前備份的 JSON 歷史檔案，整合過往紀錄。
* 📊 **躍遷數據分析**：分析抽卡數據，計算每次出貨花費的抽數。
* 📤 **資料匯出備份**：將躍遷紀錄匯出為 JSON 格式檔案，方便未來備份與移植。

---

## 🛠️ 系統需求與下載 (Prerequisites & Download)

* **作業系統**：Windows 10 / 11 (**僅支援 64-bit / x64**)
* **免安裝環境**：本程式採用 ReadyToRun (R2R) 技術編譯，**無需安裝 .NET SDK 或 Runtime**，下載即可直接執行。

---

## 🚀 如何使用 (How to Use)

1. 前往本專案的 [Releases](../../releases) 頁面下載最新版本的 `StarRailWarpTracker.exe` 執行檔。
2. **強烈建議將程式放在一個獨立資料夾內**（例如：新建一個名為 `StarRailWarpTracker` 的資料夾）。
3. 雙擊執行 `StarRailWarpTracker.exe`。
4. 程式啟動後會顯示選單，請輸入對應數字選擇所需功能。

> 💡 **小提示**：自動抓取紀錄時，請確保在24小時內有開啟過遊戲內的「躍遷紀錄」頁面，以利程式讀取日誌資料。

---

## 📁 檔案儲存結構 (Directory Structure)

程式會自動將躍遷紀錄依照 `UID` 分類，並以匯出當日日期 `yyyy-mm-dd.json` 進行命名儲存：

```text
StarRailWarpTracker/
├── StarRailWarpTracker.exe
└── Warp History/              <-- 自動建立的主資料夾
    ├── 100234567/             <-- 依玩家 UID 建立資料夾
    │   ├── 2026-03-30.json    <-- 依日期命名的紀錄檔
    │   └── 2026-07-28.json
    └── 800987654/
        └── 2026-07-28.json
```

## 📜 版本紀錄 (Version History)
### v1.0.0 (Initial Release)
* 首次發布！
* 支援自動抓取遊戲躍遷紀錄。
* 支援 JSON 檔案之匯入與匯出備份。
* 提供基礎躍遷統計與分析功能。

## 📄 授權條款 (License)
本專案採用 [MIT License](LICENSE) 授權發布。
