import * as fs from 'fs';
import * as path from 'path';
import { FileSystem } from "./filesystem";

export class LocalFileSystem extends FileSystem {

	async deployDemo(sourcePath: string, destinationPath: string): Promise<void> {
		if (!fs.existsSync(destinationPath)) {
			fs.mkdirSync(destinationPath, { recursive: true });
		}
	
		await fs.readdir(sourcePath, { withFileTypes: true }, 
			async (error, entries) => {
				if (!error) await this.deployEntries(entries, sourcePath, destinationPath);
			}
		);
	}

	private async deployEntries(entries: fs.Dirent[], sourcePath: string, destinationPath: string) : Promise<void> {	
		for (const entry of entries) {
            
            if (!this.shouldCopy(entry)) { continue; }

			const finalSourcePath = path.join(sourcePath, entry.name);
			const finalDestinationPath = path.join(destinationPath, entry.name);
	
			if (entry.isDirectory()) {
				this.deployDemo(finalSourcePath, finalDestinationPath);
			} else if (entry.isFile()) {
				fs.copyFileSync(finalSourcePath, finalDestinationPath);
			}
		}

	}

	async clearDirectoryRecursive(directoryPath: string): Promise<void> {
		if (!fs.existsSync(directoryPath)) { return; }

		const entries = fs.readdirSync(directoryPath, { withFileTypes: true });
	
		for (const entry of entries) {
			const entryPath = path.join(directoryPath, entry.name);
	
			if (entry.isDirectory()) {
				await fs.rmdir(entryPath, { recursive: true }, () => {});
			} else if (entry.isFile()) {
				await fs.unlink(entryPath, () => {});
			}
		}
	}

	async ensureDirectoryExists(fullPath: string): Promise<string>;

	async ensureDirectoryExists(root: string, directory?: string): Promise<string> {
		const fullPath = directory ? path.join(root, directory) : root;

		if (fs.existsSync(fullPath)) { return fullPath; }
		await fs.mkdir(fullPath, { recursive: true }, () => { });

		return fullPath;
	}

	async ensureTextFileExists(path: string, defaultContent?: string): Promise<string>;

	async ensureTextFileExists(root: string, fileName?: string, defaultContent?: string): Promise<string> {
		const fullPath = fileName ? path.join(root, fileName) : root;
		if (fs.existsSync(fullPath)) { return fullPath; }

		await fs.writeFile(fullPath, defaultContent || '', () => { });

		return fullPath;
	}

    private shouldCopy(entry: fs.Dirent): boolean {
        if (entry.name.startsWith('.')) { return false; }
        if (entry.isDirectory() && this.ignoreDirectories.includes(entry.name)) { return false; }
        return true;
    }

    private get ignoreDirectories(): string[] {
        return ['bin', 'obj'];
    }
}