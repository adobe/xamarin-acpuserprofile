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
using Com.Adobe.Marketing.Mobile;
using System.Collections.Generic;
using System.Threading;
using Android.Runtime;

namespace ACPUserProfileAndroidUnitTests
{
    [TestFixture]
    public class ACPUserProfileAndroidUnitTests
    {
        // CountDownEvent latch
        static CountdownEvent latch;

        // static string to store data retrieved via callback
        private static string callbackString = "";

        [SetUp]
        public void Setup()
        {
            latch = null;
            callbackString = null;
        }

        // ACPUserProfile tests
        [Test]
        public void GetACPUserProfileExtensionVersion_Returns_CorrectVersion()
        {
            // verify
            Assert.That(ACPUserProfile.ExtensionVersion(), Is.EqualTo("1.1.0"));
        }

        [Test]
        public void UpdateUserAttribute_Then_GetUserAttributes_Returns_ExpectedAttributes()
        {
            // setup
            latch = new CountdownEvent(1);
            // test
            ACPUserProfile.UpdateUserAttribute("key", "value");
            var attributes = new List<string>();
            attributes.Add("key");
            ACPUserProfile.GetUserAttributes(attributes, new AdobeCallback());
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
            var attributes = new List<string>();
            attributes.Add("key");
            ACPUserProfile.GetUserAttributes(attributes, new AdobeCallback());
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
            var attributes = new Dictionary<string, Java.Lang.Object>();
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");
            ACPUserProfile.UpdateUserAttributes(attributes);
            var attributeKeys = new List<string>();
            attributeKeys.Add("key1");
            attributeKeys.Add("key2");
            ACPUserProfile.GetUserAttributes(attributeKeys, new AdobeCallback());
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
            var attributes = new Dictionary<string, Java.Lang.Object>();
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");
            ACPUserProfile.UpdateUserAttributes(attributes);
            ACPUserProfile.RemoveUserAttribute("key1");
            var attributeKeys = new List<string>();
            attributeKeys.Add("key1");
            attributeKeys.Add("key2");
            ACPUserProfile.GetUserAttributes(attributeKeys, new AdobeCallback());
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
            var attributes = new Dictionary<string, Java.Lang.Object>();
            attributes.Add("key1", "value1");
            attributes.Add("key2", "value2");
            attributes.Add("key3", "value3");
            attributes.Add("key4", "value4");
            ACPUserProfile.UpdateUserAttributes(attributes);
            var keysToRemove = new List<string>();
            keysToRemove.Add("key1");
            keysToRemove.Add("key3");
            ACPUserProfile.RemoveUserAttributes(keysToRemove);
            var keysToRetrieve = new List<string>();
            keysToRetrieve.Add("key1");
            keysToRetrieve.Add("key2");
            keysToRetrieve.Add("key3");
            keysToRetrieve.Add("key4");
            ACPUserProfile.GetUserAttributes(keysToRetrieve, new AdobeCallback());
            latch.Wait(1000);
            // verify
            Assert.That(callbackString.Contains("[ key2 : value2 ]"));
            Assert.That(callbackString.Contains("[ key4 : value4 ]"));
        }

        // callbacks
        class AdobeCallback : Java.Lang.Object, IAdobeCallbackWithError
        {
            public void Fail(AdobeError error)
            {
                callbackString = "";
                String message = "GetUserAttributes error:" + error.ToString();
                callbackString = message;
            }

            public void Call(Java.Lang.Object retrievedAttributes)
            {
                callbackString = "";
                if (retrievedAttributes != null)
                {
                    var attributesDictionary = new Android.Runtime.JavaDictionary<string, object>(retrievedAttributes.Handle, Android.Runtime.JniHandleOwnership.DoNotRegister);
                    foreach (KeyValuePair<string, object> pair in attributesDictionary)
                    {
                        callbackString = callbackString + "[ " + pair.Key + " : " + pair.Value + " ]";
                    }
                }
                else
                {
                    callbackString = "GetUserAttributes callback is null.";
                }
                if (latch != null)
                {
                    latch.Signal();
                }
            }
        }
    }
}