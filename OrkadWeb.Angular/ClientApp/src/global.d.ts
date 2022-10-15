declare global {
  interface Array<T> {
    /** Remove the first matching element in the array */
    remove(item: T): void;
  }
}

export {};
