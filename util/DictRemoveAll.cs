using System;
using System.Collections.Generic;

namespace evilbug.util;

public static class DictRemoveAll {
  public static void RemoveAll<T, U>(this Dictionary<T, U> dict, Predicate<T> predicate) {
    HashSet<T> keys_dispose = [];
    foreach (T key in dict.Keys) {
      if (predicate(key)) {
        keys_dispose.Add(key);
      }
    }

    foreach (T key in keys_dispose) {
      dict.Remove(key);
    }
  }
}