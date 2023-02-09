# Makefile requires Visual Studio for Mac Community version to be installed
# Tested with 8.5.3 (build 16)
setup:
	cd src/Adobe.ACPUserProfile.Android/ && msbuild -t:restore
	cd src/Adobe.ACPUserProfile.iOS/ && msbuild -t:restore

msbuild-clean:
	cd src && msbuild -t:clean

clean-folders:
	rm -rf src/Adobe.ACPUserProfile.Android/obj
	rm -rf src/Adobe.ACPUserProfile.Android/bin/Debug
	rm -rf src/Adobe.ACPUserProfile.iOS/bin/Debug
	rm -rf src/Adobe.ACPUserProfile.iOS/obj
	rm -rf bin

clean: msbuild-clean clean-folders setup

# Makes ACPUserProfile bindings and NuGet packages. The bindings (.dll) will be available in BindingDirectory/bin/Debug
# The NuGet packages get created in the same directory but are then moved to src/bin.
release:
	cd src/Adobe.ACPUserProfile.Android/ && msbuild -t:build
	cd src/Adobe.ACPUserProfile.iOS/ && msbuild -t:build
	cd src/Adobe.ACPUserProfile.iOS/ && msbuild -t:pack	
	mkdir bin
	cp src/Adobe.ACPUserProfile.Android/bin/Debug/*.nupkg ./bin
	cp src/Adobe.ACPUserProfile.iOS/bin/Debug/*.nupkg ./bin

ACPUSERPROFILE_SDK_PATH = ./acp-sdk
ACPUSERPROFILE_SDK_IOS_USERPROFILE_PATH = ./acp-sdk/iOS/ACPUserProfile
UNIVERSAL_USERPROFILE_IOS_PATH = ./acp-sdk/universal-acpuserprofile-ios
UNIVERSAL_USERPROFILE_IOS_ACPUSERPROFILE_PATH = ./acp-sdk/universal-acpuserprofile-ios/ACPUserProfile
SIMULATOR_DIRECTORY_NAME = ios-arm64_i386_x86_64-simulator
DEVICE_DIRECTORY_NAME = ios-arm64_armv7_armv7s

download-acp-sdk:
	mkdir -p $(ACPUSERPROFILE_SDK_PATH)
	git clone --depth 1 https://github.com/Adobe-Marketing-Cloud/acp-sdks.git $(ACPUSERPROFILE_SDK_PATH)

update-userprofile-ios-static-libraries:
	mkdir -p $(UNIVERSAL_USERPROFILE_IOS_PATH)
	mv $(ACPUSERPROFILE_SDK_IOS_USERPROFILE_PATH) $(UNIVERSAL_USERPROFILE_IOS_PATH)
	lipo -remove arm64 -output $(UNIVERSAL_USERPROFILE_IOS_ACPUSERPROFILE_PATH)/ACPUserProfile.xcframework/$(SIMULATOR_DIRECTORY_NAME)/libACPUserProfile_iOS_clean.a $(UNIVERSAL_USERPROFILE_IOS_ACPUSERPROFILE_PATH)/ACPUserProfile.xcframework/$(SIMULATOR_DIRECTORY_NAME)/libACPUserProfile_iOS.a
	lipo -create $(UNIVERSAL_USERPROFILE_IOS_ACPUSERPROFILE_PATH)/ACPUserProfile.xcframework/$(DEVICE_DIRECTORY_NAME)/libACPUserProfile_iOS.a $(UNIVERSAL_USERPROFILE_IOS_ACPUSERPROFILE_PATH)/ACPUserProfile.xcframework/$(SIMULATOR_DIRECTORY_NAME)/libACPUserProfile_iOS_clean.a  -output $(UNIVERSAL_USERPROFILE_IOS_PATH)/libACPUserProfile_iOS.a
	mv $(UNIVERSAL_USERPROFILE_IOS_PATH)/libACPUserProfile_iOS.a ./src/Adobe.ACPUserProfile.iOS
