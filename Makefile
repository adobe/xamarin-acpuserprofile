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
	cd src/Adobe.ACPUserProfile.Android/ && msbuild -t:pack	
	cd src/Adobe.ACPUserProfile.iOS/ && msbuild -t:build
	cd src/Adobe.ACPUserProfile.iOS/ && msbuild -t:pack	
	mkdir bin
	cp src/Adobe.ACPUserProfile.Android/bin/Debug/*.nupkg ./bin
	cp src/Adobe.ACPUserProfile.iOS/bin/Debug/*.nupkg ./bin
