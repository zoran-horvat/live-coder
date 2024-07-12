import { Dialogs } from "./dialogs";
import { Environment } from "./environment";

export abstract class Ide {
	abstract get dialogs(): Dialogs;
    abstract get environment(): Environment;

	abstract withNoEditorsOpen() : Promise<void>;
}