import * as fs from 'fs';
import * as path from 'path';
import { FileSystem } from "./filesystem";

interface DirectoryCopy { 
	sourcePath: string,
	destinationPath: string
}

export class LocalFileSystem extends FileSystem {
	async deployDemo(sourcePath: string, destinationPath: string): Promise<void> {
		if (!fs.existsSync(destinationPath)) { fs.mkdirSync(destinationPath); }

		for (var entry of fs.readdirSync(sourcePath, { withFileTypes: true })) {
			if (!this.shouldCopy(entry)) { continue; }

			const entrySourcePath = path.join(sourcePath, entry.name);
			const entryDestinationPath = path.join(destinationPath, entry.name);

			if (entry.isDirectory()) {
				this.deployDemo(entrySourcePath, entryDestinationPath)
			} else if (entry.isFile()) { 
				fs.copyFileSync(entrySourcePath, entryDestinationPath);
			}
		}
	}

	async clearDirectoryRecursive(directoryPath: string): Promise<void> {
		if (!fs.existsSync(directoryPath)) { return; }

		for (const entry of fs.readdirSync(directoryPath, { withFileTypes: true })) {
			const entryPath = path.join(directoryPath, entry.name);
	
			if (entry.isDirectory()) {
				await fs.rmdirSync(entryPath, { recursive: true });
			} else if (entry.isFile()) {
				await fs.unlinkSync(entryPath);
			}
		}
	}

	private asString(err : any) : string {
		if (err instanceof Error) { return err.message; }
		return JSON.stringify(err);
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

		await fs.writeFileSync(fullPath, defaultContent || '');

		return fullPath;
	}

	async getExistingFilePath(root: string, fileName: string) : Promise<string | null> {
		const fullPath = path.join(root, fileName);
		if (fs.existsSync(fullPath)) { return Promise.resolve(fullPath); }
		return Promise.resolve(null);
	}

    private shouldCopy(entry: fs.Dirent): boolean {
        if (entry.name.startsWith('.')) { return false; }
        if (entry.isDirectory() && this.ignoreDirectories.includes(entry.name)) { return false; }
        return true;
    }

    private get ignoreDirectories(): string[] {
        return ['bin', 'obj'];
    }

	public loadJsonFile(path: string) : Promise<any | null> {
		const fileContent = fs.readFileSync(path);
		try {
			const obj = JSON.parse(fileContent.toString());
			return Promise.resolve(obj);
		} catch (error) { }	
		return Promise.resolve(null);
	}
}