import { Dialogs } from "../ide-integration/dialogs";
import { Ide } from "../ide-integration/ide";
import { VsCodeDialogs } from "./dialogs";

export class VsCodeClient extends Ide {
	get dialogs() : Dialogs { return new VsCodeDialogs(); }
}