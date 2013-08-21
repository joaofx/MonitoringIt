require 'albacore'
require 'fileutils'

# actual version
VERSION = '0.1.2'

# dirs
current_dir = File.dirname(__FILE__)
build_dir = "#{current_dir}/build"
package_dir = "build/package"

# variables
nuget_exe = "tools/nuget/NuGet.exe"
nuspec_file = "#{build_dir}/release/MonitoringIt.nuspec"

# tasks
task :default => [ :prepare, :compile, ]
task :package => [ :assemblyinfo, :compile, :copy_release, :spec, :pack ]
task :release => [ :package, :push ]

task :prepare do
	FileUtils.rm_rf('build')

	Dir.mkdir(build_dir)
	Dir.mkdir("#{build_dir}/release")
	Dir.mkdir("#{build_dir}/release/lib")
	Dir.mkdir("#{build_dir}/package")
end

assemblyinfo :assemblyinfo do |asm|
  asm.version = VERSION
  asm.company_name = "MonitoringIt"
  asm.product_name = "MonitoringIt"
  asm.title = "MonitoringIt"
  asm.description = "MonitoringIt"
  asm.output_file = "#{current_dir}/src/MonitoringIt/Properties/AssemblyInfo.cs"
end

msbuild :compile do |msb|
  msb.properties :configuration => :Release, :outdir  => "#{build_dir}/bin"
  msb.targets = [ :Clean, :Build ]
  msb.solution = "#{current_dir}/src/MonitoringIt.sln"
  msb.verbosity = "minimal"
end

task :copy_release do
	FileUtils.cp "#{build_dir}/bin/MonitoringIt.dll", "#{build_dir}/release/lib/MonitoringIt.dll"
end

nuspec :spec do |nuspec|
	nuspec.id = "MonitoringIt"
	nuspec.version = VERSION
	nuspec.authors = "joaofx"
	nuspec.owners = "joaofx"
	nuspec.description = "A lib that helps you write performance counters easily"
	nuspec.title = "MonitoringIt"
	nuspec.projectUrl = "https://github.com/joaofx/MonitoringIt"
	nuspec.output_file = "MonitoringIt.nuspec"
	nuspec.working_directory = "#{build_dir}/release"
end
  
nugetpack :pack do |nuget|
   nuget.command     = "tools/nuget/NuGet.exe"
   nuget.nuspec      = "#{build_dir}/release/MonitoringIt.nuspec"
   nuget.output      = "build/package"
end

nugetpush :push do |nuget|
 
	require './config/nuget.rb'
	
    nuget.command = nuget_exe
	nuget.package = "#{build_dir}/package/MonitoringIt.#{VERSION}.nupkg"
    nuget.apikey = API_KEY
	
end