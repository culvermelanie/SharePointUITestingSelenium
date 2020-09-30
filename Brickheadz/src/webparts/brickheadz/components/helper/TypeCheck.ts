export class TypeCheck {

  public static checkType(type: string): boolean {
    switch (type) {
      case "boy":
      case "girl":
      case "all":
        return true;
      default:
        return false;
    }
  }

}
