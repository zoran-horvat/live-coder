import * as fs from 'fs';
import * as path from 'path';
import { FileSystem } from './FileSystem';

export class Implementation extends FileSystem {

	copySourceCode(source: string, destination: string) {
		if (!fs.existsSync(destination)) {
			fs.mkdirSync(destination, { recursive: true });
		}

		const entries = fs.readdirSync(source, { withFileTypes: true });

		for (const entry of entries) {
			const sourcePath = path.join(source, entry.name);
			const destPath = path.join(destination, entry.name);

			if (entry.isDirectory()) {
				this.copySourceCode(sourcePath, destPath);
			} else if (entry.isFile()) {
				fs.copyFileSync(sourcePath, destPath);
			}
		}
	}

	clearDirectoryRecursive(directory: string) {
		if (!fs.existsSync(directory)) { return; }

		const entries = fs.readdirSync(directory, { withFileTypes: true });

		for (const entry of entries) {
			const entryPath = path.join(directory, entry.name);
			this.delete(entry);
		}
	}

	private delete(item: fs.Dirent) {
		if (item.isDirectory()) {
			this.clearDirectoryRecursive(item.name);
			fs.rmdirSync(item.name);
		}
		else if (item.isFile()) {
			fs.unlinkSync(item.name);
		}
	}
}