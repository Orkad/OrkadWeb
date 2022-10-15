Array.prototype.remove = function <T>(this: T[], item: T): void {
  const index = this.indexOf(item, 0);
  if (index > -1) {
    this.splice(index, 1);
  }
};
