import { FileSystem } from "../fs-integration/filesystem";
import { Environment } from "../ide-integration/environment";
import { Ide } from "../ide-integration/ide";

export async function command(ide: Ide, fs: FileSystem, environment: Environment) : Promise<void> {
    const sourcePath = await ide.dialogs.selectDirectoryOrShowError('Select Source Directory', "No source directory selected.", environment.lastRecordingPath);
	if (!sourcePath) { return; }

	environment.lastRecordingPath = sourcePath;

	const scriptDir = await fs.ensureDirectoryExists(sourcePath, '.live-coder');
	const scriptFile = await fs.ensureJsonFileExists(scriptDir, 'demo.lcs', '{ "title": "Demo", "script": {} }');

	const destinationPath = await ide.dialogs.selectDirectoryOrShowError('Select Demo Directory', 'No demo directory selected.', environment.lastDestPath);
	if (!destinationPath) { return; }

	environment.lastDestPath = destinationPath;

	const redirectScriptDir = await fs.ensureDirectoryExists(destinationPath, '.live-coder');
	const redirectScriptContent = '{ "script": { "redirect": "' + scriptFile + '" } }';
	const redirectScriptFile = await fs.ensureJsonFileExists(redirectScriptDir, 'demo.lcs', redirectScriptContent);

	await ide.withScriptEditorActive();
}
