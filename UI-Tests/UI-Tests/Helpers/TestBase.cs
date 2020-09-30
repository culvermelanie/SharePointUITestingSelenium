using Microsoft.SharePoint.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITests.Helpers
{
    public abstract class TestBase
    {
        protected TestContext testContextInstance;
        protected IWebDriver driver;
        protected string _appURL;
        protected TimeSpan timeoutControls = new TimeSpan(0, 0, 60);
        protected WebDriverWait Waiter { get; set; }



        protected void CaptureScreenShot()
        {
            this.testContextInstance.WriteLine("Capture Screenshot");
            string fileName = "TestAutomation_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm") + ".jpeg";
            string file = TestContext.TestDir;
            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(file + fileName);
            //SharePoint Online - Credentials  
            string siteUrl = (string)TestContext.Properties["SiteUrl"];
            string userName = (string)TestContext.Properties["Username"];
            string password = (string)TestContext.Properties["Password"];
            OfficeDevPnP.Core.AuthenticationManager authManager = new OfficeDevPnP.Core.AuthenticationManager();
            ClientContext context = authManager.GetSharePointOnlineAuthenticatedContextTenant(siteUrl, userName, password);
            // get the root folder
            var library = context.Web.Lists.GetByTitle("Documents");
            context.Load(library, l => l.RootFolder);
            context.ExecuteQuery();

            // upload the file
            library.RootFolder.UploadFileAsync(fileName, file + fileName, false);
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestInitialize()]
        public void SetupTest()
        {
            _appURL = (string)TestContext.Properties["SiteUrl"];

            string browser = (string)TestContext.Properties["Browser"];
            switch (browser)
            {
                case "Chrome":
                    ChromeOptions options = new ChromeOptions();
                    options.AddArgument("--incognito");
                    driver = new ChromeDriver(options);
                    break;
                case "IE":
                    InternetExplorerOptions optionsIE = new InternetExplorerOptions();
                    optionsIE.BrowserCommandLineArguments = " -private";
                    driver = new InternetExplorerDriver(optionsIE);
                    break;
                case "Edge":
                    driver = new EdgeDriver();
                    break;
                default:
                    driver = new ChromeDriver();
                    break;
            }

            driver.Manage().Window.Maximize();
            Waiter = new WebDriverWait(driver, this.timeoutControls);

            Authenticate();

        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            driver.Quit();
        }

        protected void WaitPageLoad()
        {
            try
            {
                System.Threading.Thread.Sleep(10000);
                //new WebDriverWait(this.driver, this.timeoutControls).Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch (Exception e)
            {
                //ToDo
            }
        }

        private void Authenticate()
        {
            this.testContextInstance.WriteLine("Authenticate");
            driver.Navigate().GoToUrl(_appURL + "/");
            //username
            Waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("input[name=\"loginfmt\"]")));
            //driver.FindElement(By.Id("otherTileText")).Click();
            driver.FindElement(By.CssSelector("input[name=\"loginfmt\"]")).Clear();
            driver.FindElement(By.CssSelector("input[name=\"loginfmt\"]")).SendKeys((string)TestContext.Properties["Username"]);
            //Waiter.Until(driver => driver.FindElement(By.Id("idSIButton9")));
            //does not work in IE
            //driver.FindElement(By.CssSelector("input[value=\"Next\"]")).Click();
            driver.FindElement(By.CssSelector("input[name=\"loginfmt\"]")).SendKeys(Keys.Return);

            //password
            //Waiter.Until(driver => driver.FindElement(By.Id("i0118")));
            //driver.FindElement(By.Id("i0118")).Click();
            Waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("input[name=\"passwd\"]")));
            var pwdField = driver.FindElement(By.CssSelector("input[name=\"passwd\"]"));
            //pwdField.Clear();            
            pwdField.SendKeys((string)TestContext.Properties["Password"]);
            pwdField.SendKeys(Keys.Return);
            //Waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("input[value=\"Sign in\"]")));
            //does not work in IE
            //driver.FindElement(By.CssSelector("input[value=\"Sign in\"]")).Click();


            //stay signed in dialog
            //Waiter.Until(driver => driver.FindElement(By.Id("idSIButton9")));
            Waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("KmsiCheckboxField")));
            driver.FindElement(By.Id("KmsiCheckboxField")).Click();

            //does not work in IE
            //driver.FindElement(By.CssSelector("input[value=\"Yes\"]")).Click();
            driver.FindElement(By.Id("KmsiCheckboxField")).SendKeys(Keys.Return);
        }
    }
}
