import * as fs from 'fs';
import * as path from 'path';

export function copyDirectoryRecursive(source: string, destination: string) {
	if (!fs.existsSync(destination)) {
		fs.mkdirSync(destination, { recursive: true });
	}

	const entries = fs.readdirSync(source, { withFileTypes: true });

	for (const entry of entries) {
		const sourcePath = path.join(source, entry.name);
		const destPath = path.join(destination, entry.name);

		if (entry.isDirectory()) {
			copyDirectoryRecursive(sourcePath, destPath);
		} else if (entry.isFile()) {
			fs.copyFileSync(sourcePath, destPath);
		}
	}
}

export function clearDirectoryRecursive(directory: string) {
	if (!fs.existsSync(directory)) { return; }

	const entries = fs.readdirSync(directory, { withFileTypes: true });

	for (const entry of entries) {
		const entryPath = path.join(directory, entry.name);

		if (entry.isDirectory()) {
			clearDirectoryRecursive(entryPath);
			fs.rmdirSync(entryPath);
		} else if (entry.isFile()) {
			fs.unlinkSync(entryPath);
		}
	}
}
