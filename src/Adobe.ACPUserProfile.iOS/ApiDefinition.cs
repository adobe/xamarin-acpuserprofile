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
using Foundation;

namespace Com.Adobe.Marketing.Mobile
{
    // @interface ACPUserProfile : NSObject
    [BaseType(typeof(NSObject))]
    interface ACPUserProfile
    {
        // +(NSString * _Nonnull)extensionVersion;
        [Static]
        [Export("extensionVersion")]
        string ExtensionVersion();

        // +(void)registerExtension;
        [Static]
        [Export("registerExtension")]
        void RegisterExtension();

        // +(void)removeUserAttribute:(NSString * _Nonnull)attributeName;
        [Static]
        [Export("removeUserAttribute:")]
        void RemoveUserAttribute(string attributeName);

        // +(void)removeUserAttributes:(NSArray<NSString *> * _Nullable)attributeNames;
        [Static]
        [Export("removeUserAttributes:")]
        void RemoveUserAttributes([NullAllowed] string[] attributeNames);

        // +(void)updateUserAttribute:(NSString * _Nonnull)attributeName withValue:(NSString * _Nullable)attributeValue;
        [Static]
        [Export("updateUserAttribute:withValue:")]
        void UpdateUserAttribute(string attributeName, [NullAllowed] string attributeValue);

        // +(void)updateUserAttributes:(NSDictionary * _Nonnull)attributeMap;
        [Static]
        [Export("updateUserAttributes:")]
        void UpdateUserAttributes(NSDictionary attributeMap);

        // +(void)getUserAttributes:(NSArray<NSString *> * _Nullable)attributNames withCompletionHandler:(void (^ _Nonnull)(NSDictionary * _Nullable, NSError * _Nullable))completionHandler;
        [Static]
        [Export("getUserAttributes:withCompletionHandler:")]
        void GetUserAttributes([NullAllowed] string[] attributNames, Action<NSDictionary, NSError> completionHandler);
    }
}
