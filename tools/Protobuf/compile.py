import subprocess
import os
import sys

config = __import__(sys.argv[1].replace('.py', ''))
BASE_DIRECTORY = os.getcwd()+"\\"

def getFile(f_name):
	return os.path.join(BASE_DIRECTORY, f_name)

def getFiles(f_names):
	return [getFile(f_name) for f_name in f_names]


def compile(f_proto, f_out):
	cmd = [config.COMPILER_PATH, "--output", f_out]
	cmd.extend(f_proto)
	if config.COMPILER_OPTIONS:
		cmd.extend(config.COMPILER_OPTIONS)
	subprocess.call(cmd, cwd=BASE_DIRECTORY)


def combine(path, file):
	return os.path.join(path, file)

def copy(source, destDir):
	os.system("copy /y \"%s\" \"%s\"" % (source, destDir))
compile(getFiles(config.INPUT_FILES), getFile(config.OUTPUT_FILE))


for path in os.listdir(BASE_DIRECTORY):
	if path.endswith(".cs"):
		for copypath in config.COPY_PATHS:
			copypath = combine(BASE_DIRECTORY, copypath)
			copy(getFile(path), combine(copypath, path).replace("\\", "/"))
		os.remove(path)