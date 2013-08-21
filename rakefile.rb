require 'albacore'
require 'fileutils'

VERSION = '0.1.1'
current_dir = File.dirname(__FILE__)
build_dir = "#{current_dir}/build"

task :default => [ :build, :copy_release, :spec, :pack ]

FileUtils.rm_rf('build')

Dir.mkdir(build_dir)
Dir.mkdir("#{build_dir}/release")
Dir.mkdir("#{build_dir}/release/lib")
Dir.mkdir("#{build_dir}/package")

msbuild :build do |msb|
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