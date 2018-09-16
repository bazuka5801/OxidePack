COMPILER_PATH = "bin/ProtoCompiler.exe"
COMPILER_OPTIONS = [ "--usings", "OxidePack" ]

INPUT_FILES = [
	"OxidePack.Common.proto",
]
OUTPUT_FILE = "OxidePack.Data.cs"
COPY_PATHS = [
	"../../src/OxidePack.Common/Data"
]