import { FileSystem } from "../fs-integration/filesystem";
import { Environment } from "../ide-integration/environment";
import { Ide } from "../ide-integration/ide";

export async function command(ide: Ide, fs: FileSystem, environment: Environment) : Promise<void> {
    const sourcePath = await ide.dialogs.selectDirectoryOrShowError('Select Source Directory', "No source directory selected.", environment.lastRecordingPath);
	if (!sourcePath) { return; }

	environment.lastRecordingPath = sourcePath;

	await fs.ensureDirectoryExists(sourcePath, '.live-coder');
}
