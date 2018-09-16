COMPILER_PATH = "bin/ProtoCompiler.exe"
COMPILER_OPTIONS = [ "--usings", "OxidePack", "--exclude-protoparser" ]

INPUT_FILES = [
	"OxidePack.Client.proto"
]
OUTPUT_FILE = "OxidePack.Data.cs"
COPY_PATHS = [
	"../../src/OxidePack.Client/Data"
]