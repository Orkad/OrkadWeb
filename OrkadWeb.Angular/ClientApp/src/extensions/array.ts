export {};
declare global {
  interface Array<T> {
    /** Remove the first matching element in the array */
    remove(item: T): void;
    /** Replace the first matching element in the array */
    replace(item: T, newValue: T): void;
  }
}

Array.prototype.remove = function <T>(this: T[], item: T): void {
  const index = this.indexOf(item, 0);
  if (index !== -1) {
    this.splice(index, 1);
  }
};
Array.prototype.replace = function <T>(this: T[], item: T, newValue: T): void {
  let index = this.indexOf(item);
  if (index !== -1) {
    this.splice(index, 1, newValue);
  }
};
