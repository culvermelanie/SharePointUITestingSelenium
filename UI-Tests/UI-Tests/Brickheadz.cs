using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UITests.Helpers;
using OpenQA.Selenium;

namespace UITests
{
    /// <summary>
    /// Summary description for Brickheadz
    /// </summary>
    [TestClass]
    public class Brickheadz : TestBase
    {
        private const string APP_NAME = "ecs-2019-pnp-client-side-solution";

        [TestMethod]
        public void VerifyAppIsInstalled()
        {
            try
            {
                driver.Navigate()
                    .GoToUrl((string)TestContext.Properties["SiteUrl"] + "/_layouts/15/viewlsts.aspx");
                base.WaitPageLoad();
                //check by fetching html element - is actually a double check, because the selector also relies on the name ;)
                Assert.AreEqual(APP_NAME, 
                    driver.FindElement(By.XPath("//a[contains(text(),'"+ APP_NAME + "')]")).Text);
            }
            catch (Exception ex)
            {
                this.CaptureScreenShot();
                Assert.Fail("Failed with following message: " + ex.ToString());
            }
        }

        [TestMethod]
        public void AddWebPartAndVerify()
        {
            try
            {
                driver.Navigate().GoToUrl((string)TestContext.Properties["SiteUrl"]);
                base.WaitPageLoad();
                //System.Threading.Thread.Sleep(new TimeSpan(0, 0, 2));
                base.Waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("button[aria-label='Edit Page']"))).Click(); //open edit mode
                
                //wait for a second so all non standard webparts are loaded
                System.Threading.Thread.Sleep(new TimeSpan(0,0,2));
                //trigger the hover action to show the + add icon
                base.Waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.CssSelector("div.CanvasSection-xl8 button[aria-label=\"Add a new web part in column one\"]"))).Click();
                //select webpart
                base.Waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("div[title='Brickheadz']"))).Click();
                
                //open webpart properties
                //click somewhere into the canvas to trigger the edit menu
                base.Waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("div[data-ui-test-id='brickheadz']"))).Click();
                base.Waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector(".CanvasZoneContainer button[data-automation-id='configureButton']"))).Click(); //button[aria-label='Edit web part']
                var textBox = base.Waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("div[aria-label='Brickheadz settings'] input")));

                //check for girls
                textBox.SendKeys("girl");                
                Assert.AreEqual(2, base.driver.FindElements(By.CssSelector("div[data-ui-test-id='brickheadz'] img")).Count);
                Assert.IsTrue(base.driver.FindElements(By.CssSelector("div[data-ui-test-id='brickheadz'] img"))[0].GetAttribute("src").Contains("girl1"));
                Assert.IsTrue(base.driver.FindElements(By.CssSelector("div[data-ui-test-id='brickheadz'] img"))[1].GetAttribute("src").Contains("girl2"));

                //check for boys
                textBox.SendKeys(Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace);
                textBox.SendKeys("boy");
                Assert.AreEqual(2, base.driver.FindElements(By.CssSelector("div[data-ui-test-id='brickheadz'] img")).Count);
                Assert.IsTrue(base.driver.FindElements(By.CssSelector("div[data-ui-test-id='brickheadz'] img"))[0].GetAttribute("src").Contains("boy1"));
                Assert.IsTrue(base.driver.FindElements(By.CssSelector("div[data-ui-test-id='brickheadz'] img"))[1].GetAttribute("src").Contains("boy2"));

                //check for all
                textBox.SendKeys(Keys.Backspace + Keys.Backspace + Keys.Backspace);
                textBox.SendKeys("all");
                Assert.AreEqual(4, base.driver.FindElements(By.CssSelector("div[data-ui-test-id='brickheadz'] img")).Count);
                Assert.IsTrue(base.driver.FindElements(By.CssSelector("div[data-ui-test-id='brickheadz'] img"))[0].GetAttribute("src").Contains("boy1"));
                Assert.IsTrue(base.driver.FindElements(By.CssSelector("div[data-ui-test-id='brickheadz'] img"))[1].GetAttribute("src").Contains("girl1"));
                Assert.IsTrue(base.driver.FindElements(By.CssSelector("div[data-ui-test-id='brickheadz'] img"))[2].GetAttribute("src").Contains("girl2"));
                Assert.IsTrue(base.driver.FindElements(By.CssSelector("div[data-ui-test-id='brickheadz'] img"))[3].GetAttribute("src").Contains("boy2"));
            }
            catch (Exception ex)
            {
                this.CaptureScreenShot();
                Assert.Fail("Failed with following message: " + ex.ToString());
            }
            finally
            {
                try
                {
                    //clean up what we did
                    //click somewhere into the canvas to trigger the edit menu
                    base.Waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("div[data-ui-test-id='brickheadz']"))).Click();
                    base.Waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("button[data-automation-id='deleteButton']"))).Click();

                    //confirm
                    base.Waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector(".ms-Dialog-main button[data-automation-id='yesButton']"))).Click();

                    //save changes
                    base.Waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Name("Republish"))).Click();
                    //wait until we see the edit again
                    base.Waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.CssSelector("button[aria-label='Edit Page']")));
                }
                catch (Exception ex)
                { //just chatch and ignore
                }
            }
        }

    }
}
