/*
 Copyright 2020 Adobe. All rights reserved.
 This file is licensed to you under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License. You may obtain a copy
 of the License at http://www.apache.org/licenses/LICENSE-2.0
 Unless required by applicable law or agreed to in writing, software distributed under
 the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR REPRESENTATIONS
 OF ANY KIND, either express or implied. See the License for the specific language
 governing permissions and limitations under the License.
*/

using System;
using NUnit.Framework;
using Foundation;
using Com.Adobe.Marketing.Mobile;
using System.Threading;
using System.Collections.Generic;

namespace ACPUserProfileiOSUnitTests
{
    [TestFixture]
    public class ACPUserProfileiOSUnitTests
    {
        // CountDownEvent latch
        static CountdownEvent latch;
        static string callbackString;
        static Action<NSDictionary, NSError> callback;

        [SetUp]
        public void Setup()
        {
            latch = null;
            callback = (content, error) =>
            {
                callbackString = "";
                if (error != null)
                {
                    callbackString = "GetUserAttributes error:" + error.DebugDescription;
                }
                else if (content == null)
                {
                    callbackString = "GetUserAttributes callback is null.";
                }
                else
                {
                    var attributesDictionary = (NSDictionary)content;
                    foreach (KeyValuePair<NSObject, NSObject> pair in attributesDictionary)
                    {
                        callbackString = callbackString + "[ " + pair.Key + " : " + pair.Value + " ]";
                    }
                    Console.WriteLine("Retrieved attributes: " + callbackString);
                }
                if (latch != null)
                {
                    latch.Signal();
                }
            };
        }

        // ACPUserProfile tests
        [Test]
        public void GetACPUserProfileExtensionVersion_Returns_CorrectVersion()
        {
            // verify
            Assert.That(ACPUserProfile.ExtensionVersion(), Is.EqualTo("2.2.0"));
        }

        [Test]
        public void UpdateUserAttribute_Then_GetUserAttributes_Returns_ExpectedAttributes()
        {
            // setup
            latch = new CountdownEvent(1);
            // test
            ACPUserProfile.UpdateUserAttribute("key", "value");
            string[] attributes = new string[] { "key" };
            ACPUserProfile.GetUserAttributes(attributes, callback);
            latch.Wait(1000);
            // verify
            Assert.That(callbackString, Is.EqualTo("[ key : value ]"));
        }

        [Test]
        public void UpdateUserAttribute_OnExisitingKey_Then_GetUserAttributes_Returns_ExpectedAttributes()
        {
            // setup
            latch = new CountdownEvent(1);
            // test
            ACPUserProfile.UpdateUserAttribute("key", "value");
            ACPUserProfile.UpdateUserAttribute("key", "newValue");
            string[] attributes = new string[] { "key" };
            ACPUserProfile.GetUserAttributes(attributes, callback);
            latch.Wait(1000);
            // verify
            Assert.That(callbackString, Is.EqualTo("[ key : newValue ]"));
        }

        [Test]
        public void UpdateUserAttributes_Then_GetUserAttributes_Returns_ExpectedAttributes()
        {
            // setup
            latch = new CountdownEvent(1);
            // test
            var attributes = new NSMutableDictionary<NSString, NSString>
            {
                ["key1"] = new NSString("value1"),
                ["key2"] = new NSString("value2")
            };
            ACPUserProfile.UpdateUserAttributes(attributes);
            string[] attributeKeys = new string[] { "key1", "key2" };
            ACPUserProfile.GetUserAttributes(attributeKeys, callback);
            latch.Wait(1000);
            // verify
            Assert.That(callbackString.Contains("[ key1 : value1 ]"));
            Assert.That(callbackString.Contains("[ key2 : value2 ]"));
        }

        [Test]
        public void UpdateUserAttributes_Then_RemoveUserAttribute_Returns_ExpectedAttributes()
        {
            // setup
            latch = new CountdownEvent(1);
            // test
            var attributes = new NSMutableDictionary<NSString, NSString>
            {
                ["key1"] = new NSString("value1"),
                ["key2"] = new NSString("value2")
            };
            ACPUserProfile.UpdateUserAttributes(attributes);
            ACPUserProfile.RemoveUserAttribute("key1");
            string[] attributeKeys = new string[] { "key1", "key2" };
            ACPUserProfile.GetUserAttributes(attributeKeys, callback);
            latch.Wait(1000);
            // verify
            Assert.That(callbackString, Is.EqualTo("[ key2 : value2 ]"));
        }

        [Test]
        public void UpdateUserAttributes_Then_RemoveUserAttributes_Returns_ExpectedAttributes()
        {
            // setup
            latch = new CountdownEvent(1);
            // test
            var attributes = new NSMutableDictionary<NSString, NSString>
            {
                ["key1"] = new NSString("value1"),
                ["key2"] = new NSString("value2"),
                ["key3"] = new NSString("value3"),
                ["key4"] = new NSString("value4")
            };
            ACPUserProfile.UpdateUserAttributes(attributes);
            string[] keysToRemove = new string[] { "key1", "key3" };
            ACPUserProfile.RemoveUserAttributes(keysToRemove);
            var keysToRetrieve = new string[] { "key1", "key2", "key3", "key4" };
            ACPUserProfile.GetUserAttributes(keysToRetrieve, callback);
            latch.Wait(1000);
            // verify
            Assert.That(callbackString.Contains("[ key2 : value2 ]"));
            Assert.That(callbackString.Contains("[ key4 : value4 ]"));
        }
    }
}