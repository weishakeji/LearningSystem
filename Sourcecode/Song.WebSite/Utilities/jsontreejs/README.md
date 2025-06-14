# jsonTree
A lightweight vanilla Javascript micro-library for making collapsible trees with JSON

## usage

  ```
  jsonTree.init(object, selector, depth);
  ```

Because jsonTree uses document.querySelector(selector), selector can either be an ID or class, just be sure to specify with the proper identifier (. for class and # for ID).

"depth" is a boolean which decides if jsonTree should apply a class to all children elements ("depth-#"). This can be used for custom styling but may increase loading times.

The JSON object must be a valid JavaScript object or a properly formatted URL(http://, etc), as XMLHttpRequests are annoying that way.
