using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WASender.Models;

namespace WASender
{
    public class WAPIHelper
    {

        public static string ReradFile()
        {
            return System.IO.File.ReadAllText(@"wapi.js");
        }

        public static void injectWapi(IWebDriver driver)
        {
            try
            {
                IJavaScriptExecutor jssend = (IJavaScriptExecutor)driver;
                jssend.ExecuteScript(ReradFile());
            }
            catch (Exception ex)
            {

            }
        }

        public static bool IsWAPIInjected(IWebDriver driver)
        {
            IJavaScriptExecutor jssend = (IJavaScriptExecutor)driver;
            string type = (string)jssend.ExecuteScript("return typeof(WAPI)");
            if (type == "object")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool sendButtonWithMessage(IWebDriver driver, MesageModel message,string toNumber,string FinalMessage)
        {
            
            string jsString = "";
            FinalMessage = FinalMessage.Replace("\n", "");
            jsString += "return await WAPI.sendButtons('" + toNumber + "@c.us', `" + FinalMessage + "`, [";

            string buttonString = "";
            foreach (var item in message.buttons)
            {
                if (buttonString != "")
                {
                    buttonString += ",";
                }
                buttonString += "{";
                buttonString += "id: '" + Guid.NewGuid() + "',";
                buttonString += "'text': '" + item .text+ "',";
                if (item.buttonTypeEnum == enums.ButtonTypeEnum.PHONE_NUMBER)
                {
                    buttonString += "'phoneNumber': '" + item.phoneNumber + "',";
                }
                if (item.buttonTypeEnum == enums.ButtonTypeEnum.URL)
                {
                    buttonString += "'url': '" + item.url + "',";
                }
                buttonString += "}";
            }

            jsString += buttonString;
            jsString += "    ])";

            IJavaScriptExecutor jssend = (IJavaScriptExecutor)driver;
            bool result = (bool)jssend.ExecuteScript(jsString);

            return result;
        }


    }
}
