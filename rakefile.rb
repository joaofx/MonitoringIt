require 'albacore'
require 'fileutils'

task :default => [ :build, :pack ]

FileUtils.rm_rf('build')

Dir.mkdir(File.dirname(__FILE__) + "/build")
Dir.mkdir(File.dirname(__FILE__) + "/build/release")

msbuild :build do |msb|
  msb.properties :configuration => :Release, :outdir  => File.dirname(__FILE__) + "/build/bin"
  msb.targets = [ :Clean, :Build ]
  msb.solution = 'src/MonitoringIt.sln'
  msb.verbosity = "minimal"
end

nugetpack :pack do |nuget|
   nuget.command     = "tools/nuget/NuGet.exe"
   nuget.nuspec      = "monitoringit.nuspec"
   nuget.output      = "build/release"
end