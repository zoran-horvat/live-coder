import * as fs from 'fs';
import * as path from 'path';
import { FileSystem } from "./filesystem";

export class LocalFileSystem extends FileSystem {

	deployDemo(sourcePath: string, destinationPath: string): void {
		if (!fs.existsSync(destinationPath)) {
			fs.mkdirSync(destinationPath, { recursive: true });
		}
	
		const entries = fs.readdirSync(sourcePath, { withFileTypes: true });
	
		for (const entry of entries) {
			const finalSourcePath = path.join(sourcePath, entry.name);
			const finalDestinationPath = path.join(destinationPath, entry.name);
	
			if (entry.isDirectory()) {
				this.deployDemo(finalSourcePath, finalDestinationPath);
			} else if (entry.isFile()) {
				fs.copyFileSync(sourcePath, finalDestinationPath);
			}
		}
		}

	clearDirectoryRecursive(directoryPath: string): void {
		if (!fs.existsSync(directoryPath)) { return; }

		const entries = fs.readdirSync(directoryPath, { withFileTypes: true });
	
		for (const entry of entries) {
			const entryPath = path.join(directoryPath, entry.name);
	
			if (entry.isDirectory()) {
				this.clearDirectoryRecursive(entryPath);
				fs.rmdirSync(entryPath);
			} else if (entry.isFile()) {
				fs.unlinkSync(entryPath);
			}
		}
	}
}