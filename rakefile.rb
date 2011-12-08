require 'albacore'

PRODUCT_NAME = "Autofac.Settings"

BUILD_PATH = File.expand_path("build")
TOOLS_PATH = File.expand_path("tools")
LIB_PATH = File.expand_path("lib")

configuration = ENV['Configuration'] || "Debug"

FEEDS = [
	#Your internal repo can go here
	"http://go.microsoft.com/fwlink/?LinkID=206669"
]

task :default => :all

task :all => [:clean,:dependencies,:build,:specs,:copy]

task :clean do
	rmtree BUILD_PATH
end

task :dependencies do
	require 'rexml/document'	
	file = File.new("packages.config")
	doc = REXML::Document.new(file)
	doc.elements.each("packages/package") do |elm|
		package=elm.attributes["id"]
		version=elm.attributes["version"]

		packagePath="#{LIB_PATH}/#{package}"
		versionInfo="#{packagePath}/version.info"
		currentVersion=IO.read(versionInfo) if File.exists?(versionInfo)
		packageExists = File.directory?(packagePath)
		
		if(!(version or packageExists) or currentVersion!= version) then
			feedsArg = FEEDS.map{ |x| "-Source " + x }.join (' ')
			versionArg = "-Version #{version}" if version
			sh "\"#{TOOLS_PATH}/nuget/nuget.exe\" Install #{package} #{versionArg} -o \"#{LIB_PATH}\" #{feedsArg} -ExcludeVersion" do |ok,results|
				File.open(versionInfo,'w'){|f| f.write(version)} if ok
			end
		end
	end
end

msbuild :build=>[:dependencies] do |msb|
	msb.properties :configuration => configuration
	msb.targets :Clean, :Build
	msb.verbosity = "minimal"
	msb.solution = "#{PRODUCT_NAME}.sln"
end

mspec :specs => [:build] do |mspec|
	mspec.command = "lib/Machine.Specifications/tools/mspec-clr4.exe"
	mspec.assemblies Dir.glob('specs/**/*Specs.dll')
end

task :copy => [:specs] do
	Dir.glob("src/**/*.csproj") do |proj|
		name=File.basename(proj,".csproj")
		puts "Copying output for #{name}"
		src=File.dirname(proj)
		dest = "#{BUILD_PATH}/#{name}/"
		mkdir_p(dest)
		cp_r("#{src}/bin/#{configuration}/.",dest)
	end
end