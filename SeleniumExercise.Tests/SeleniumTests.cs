using System;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumExercise.Tests
{
    public class SeleniumTests
    {
        private const string BaseUrl = "https://demowf.aspnetawesome.com/";
        private readonly IWebDriver _webDriver;
        private readonly WebDriverWait _wait;

        public SeleniumTests()
        {
            _webDriver = new ChromeDriver("Drivers\\");
            _wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(1));
        }

        [OneTimeSetUp]
        public void Setup()
        {
            _webDriver.Navigate().GoToUrl(BaseUrl);
            _webDriver.Manage().Window.Maximize();
            var cookies = _wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//div[@id = 'cookieMsg']/button[@id = 'btnCookie']")));
            cookies.Click();
        }

        [Test]
        public void SelectDateInFuture_FutureDateSelected_DateDisplayedCorrectly()
        {
            // Arrange & Act
            _webDriver.FindElement(By.Id("ContentPlaceHolder1_Date1")).Click();

            var datePickerContainer = _webDriver.FindElement(By.Id("ContentPlaceHolder1_Date1-awepw"));
            var monthSelector = datePickerContainer.FindElement(By.XPath("//div[contains(@class, 'o-mhd')]/div"));
            var yearSelector = datePickerContainer.FindElement(By.XPath("//div[contains(@class, 'o-yhd')]/div"));

            monthSelector.Click();
            var monthsContainer = _webDriver.FindElement(By.Id("ContentPlaceHolder1_Date1cm-dropmenu"));
            var months = monthsContainer.FindElements(By.TagName("li"));
            months.SingleOrDefault(m => m.Text == "August")?.Click();

            yearSelector.Click();
            var yearsContainer = _webDriver.FindElement(By.Id("ContentPlaceHolder1_Date1cy-dropmenu"));
            var years = yearsContainer.FindElements(By.TagName("li"));
            years.SingleOrDefault(m => m.Text == "2026")?.Click();

            var table = datePickerContainer.FindElement(By.TagName("table"));
            var days = table.FindElements(By.XPath("//tr[position()>1]/td/div"));
            days.SingleOrDefault(d => d.Text == "22")?.Click();
        }

        [Test]
        public void SelectItemFromComboBox_ComboBoxItemSelected_ItemDisplayed()
        {
            // Arrange & Act
            var item = "Mango";
            var comboBoxContainer = _webDriver.FindElement(By.ClassName("combobox"));
            comboBoxContainer.FindElement(By.TagName("button")).Click();

            var itemsContainer = _webDriver.FindElement(By.Id("ContentPlaceHolder1_AllMealsCombo-dropmenu"));
            var items = itemsContainer.FindElements(By.TagName("li"));
            items.SingleOrDefault(i => i.Text == item)?.Click();
        }

        [Test]
        public void SelectAllAjaxCheckBoxes_AllCheckboxesSelected_AllCheckboxesShownAsSelected()
        {
            // Arrange & Act
            var checkboxContainer = _webDriver.FindElement(By.XPath("//div[contains(@class, 'awe-display o-ochk')]"));
            var checkBoxes = checkboxContainer.FindElements(By.TagName("li"));

            foreach (var checkbox in checkBoxes)
            {
                var attribute = checkbox.GetAttribute("class");
                if (!attribute.Contains("o-chked"))
                {
                    checkbox.Click();
                }
            }
        }

        [Test]
        public void SetGridPageSizeTo100_SetPageSizeTo100_NavigateToLastPage()
        {
            // Arrange & Act
            var gridContainer = _webDriver.FindElement(By.Id("ContentPlaceHolder1_Grid1"));
            var footer = gridContainer.FindElement(By.ClassName("awe-footer"));

            var pageSizeButton = _webDriver.FindElement(By.XPath("//button[@id = 'ContentPlaceHolder1_Grid1PageSize-awed']"));
            pageSizeButton.Click();

            var gridDropDown = _webDriver.FindElement(By.Id("ContentPlaceHolder1_Grid1PageSize-dropmenu"));
            var pageSizeOptions = gridDropDown.FindElements(By.TagName("li"));
            pageSizeOptions.SingleOrDefault(opt => opt.GetAttribute("data-val") == "100")?.Click();

            Thread.Sleep(500);
            var pager = footer.FindElement(By.XPath("//div[@class = 'awe-pager']"));
            var buttons = pager.FindElements(By.TagName("button"));
            buttons.LastOrDefault()?.Click();
        }
    }
}