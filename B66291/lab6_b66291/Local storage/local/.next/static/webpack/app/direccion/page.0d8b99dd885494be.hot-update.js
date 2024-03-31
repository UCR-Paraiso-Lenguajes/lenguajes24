"use strict";
/*
 * ATTENTION: An "eval-source-map" devtool has been used.
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file with attached SourceMaps in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
self["webpackHotUpdate_N_E"]("app/direccion/page",{

/***/ "(app-pages-browser)/./src/app/direccion/page.js":
/*!***********************************!*\
  !*** ./src/app/direccion/page.js ***!
  \***********************************/
/***/ (function(module, __webpack_exports__, __webpack_require__) {

eval(__webpack_require__.ts("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react/jsx-dev-runtime */ \"(app-pages-browser)/./node_modules/next/dist/compiled/react/jsx-dev-runtime.js\");\n/* harmony import */ var _styles_direccion_css__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../styles/direccion.css */ \"(app-pages-browser)/./src/styles/direccion.css\");\n/* harmony import */ var bootstrap_dist_css_bootstrap_min_css__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! bootstrap/dist/css/bootstrap.min.css */ \"(app-pages-browser)/./node_modules/bootstrap/dist/css/bootstrap.min.css\");\n/* __next_internal_client_entry_do_not_use__ default auto */ \n\n\nconst Direccion = ()=>{\n    const storedData = localStorage.getItem(\"tienda\"); // Obtiene datos locales\n    // console.log(storedData);\n    const dataObject = JSON.parse(storedData); // Convierte a objeto JavaScript para acceder a la propiedad\n    const handleSubmit = (e)=>{\n        e.preventDefault();\n        const updatedDirection = e.target.direccion.value;\n        const updatedCart = {\n            ...dataObject.cart,\n            direccionEntrega: updatedDirection\n        };\n        const updatedDataObject = {\n            ...dataObject,\n            cart: updatedCart\n        };\n        localStorage.setItem(\"tienda\", JSON.stringify(updatedDataObject));\n        // Actualizar el estado o redirigir si es necesario\n        window.location.reload(); // Recargar la página para reflejar los cambios\n    };\n    return /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"article\", {\n        children: [\n            /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"div\", {\n                className: \"form_direccion\",\n                children: /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"form\", {\n                    onSubmit: handleSubmit,\n                    children: [\n                        /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"label\", {\n                            htmlFor: \"direccion\",\n                            children: \"Direcci\\xf3n de entrega:\"\n                        }, void 0, false, {\n                            fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\direccion\\\\page.js\",\n                            lineNumber: 27,\n                            columnNumber: 11\n                        }, undefined),\n                        /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"input\", {\n                            type: \"text\",\n                            id: \"direccion\",\n                            placeholder: \"Ingresa tu direcci\\xf3n\"\n                        }, void 0, false, {\n                            fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\direccion\\\\page.js\",\n                            lineNumber: 28,\n                            columnNumber: 11\n                        }, undefined),\n                        /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"button\", {\n                            type: \"submit\",\n                            children: \"Asignar\"\n                        }, void 0, false, {\n                            fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\direccion\\\\page.js\",\n                            lineNumber: 33,\n                            columnNumber: 11\n                        }, undefined)\n                    ]\n                }, void 0, true, {\n                    fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\direccion\\\\page.js\",\n                    lineNumber: 26,\n                    columnNumber: 9\n                }, undefined)\n            }, void 0, false, {\n                fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\direccion\\\\page.js\",\n                lineNumber: 25,\n                columnNumber: 7\n            }, undefined),\n            /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"div\", {\n                className: \"cart_box\",\n                children: /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"a\", {\n                    href: \"/pago\" // URL a la página de checkout\n                    ,\n                    className: \"btn btn-info mt-3\",\n                    children: \"Continuar con la compra\"\n                }, void 0, false, {\n                    fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\direccion\\\\page.js\",\n                    lineNumber: 37,\n                    columnNumber: 9\n                }, undefined)\n            }, void 0, false, {\n                fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\direccion\\\\page.js\",\n                lineNumber: 36,\n                columnNumber: 7\n            }, undefined)\n        ]\n    }, void 0, true, {\n        fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\direccion\\\\page.js\",\n        lineNumber: 24,\n        columnNumber: 5\n    }, undefined);\n};\n_c = Direccion;\n/* harmony default export */ __webpack_exports__[\"default\"] = (Direccion);\nvar _c;\n$RefreshReg$(_c, \"Direccion\");\n\n\n;\n    // Wrapped in an IIFE to avoid polluting the global scope\n    ;\n    (function () {\n        var _a, _b;\n        // Legacy CSS implementations will `eval` browser code in a Node.js context\n        // to extract CSS. For backwards compatibility, we need to check we're in a\n        // browser context before continuing.\n        if (typeof self !== 'undefined' &&\n            // AMP / No-JS mode does not inject these helpers:\n            '$RefreshHelpers$' in self) {\n            // @ts-ignore __webpack_module__ is global\n            var currentExports = module.exports;\n            // @ts-ignore __webpack_module__ is global\n            var prevSignature = (_b = (_a = module.hot.data) === null || _a === void 0 ? void 0 : _a.prevSignature) !== null && _b !== void 0 ? _b : null;\n            // This cannot happen in MainTemplate because the exports mismatch between\n            // templating and execution.\n            self.$RefreshHelpers$.registerExportsForReactRefresh(currentExports, module.id);\n            // A module can be accepted automatically based on its exports, e.g. when\n            // it is a Refresh Boundary.\n            if (self.$RefreshHelpers$.isReactRefreshBoundary(currentExports)) {\n                // Save the previous exports signature on update so we can compare the boundary\n                // signatures. We avoid saving exports themselves since it causes memory leaks (https://github.com/vercel/next.js/pull/53797)\n                module.hot.dispose(function (data) {\n                    data.prevSignature =\n                        self.$RefreshHelpers$.getRefreshBoundarySignature(currentExports);\n                });\n                // Unconditionally accept an update to this module, we'll check if it's\n                // still a Refresh Boundary later.\n                // @ts-ignore importMeta is replaced in the loader\n                module.hot.accept();\n                // This field is set when the previous version of this module was a\n                // Refresh Boundary, letting us know we need to check for invalidation or\n                // enqueue an update.\n                if (prevSignature !== null) {\n                    // A boundary can become ineligible if its exports are incompatible\n                    // with the previous exports.\n                    //\n                    // For example, if you add/remove/change exports, we'll want to\n                    // re-execute the importing modules, and force those components to\n                    // re-render. Similarly, if you convert a class component to a\n                    // function, we want to invalidate the boundary.\n                    if (self.$RefreshHelpers$.shouldInvalidateReactRefreshBoundary(prevSignature, self.$RefreshHelpers$.getRefreshBoundarySignature(currentExports))) {\n                        module.hot.invalidate();\n                    }\n                    else {\n                        self.$RefreshHelpers$.scheduleUpdate();\n                    }\n                }\n            }\n            else {\n                // Since we just executed the code for the module, it's possible that the\n                // new exports made it ineligible for being a boundary.\n                // We only care about the case when we were _previously_ a boundary,\n                // because we already accepted this update (accidental side effect).\n                var isNoLongerABoundary = prevSignature !== null;\n                if (isNoLongerABoundary) {\n                    module.hot.invalidate();\n                }\n            }\n        }\n    })();\n//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiKGFwcC1wYWdlcy1icm93c2VyKS8uL3NyYy9hcHAvZGlyZWNjaW9uL3BhZ2UuanMiLCJtYXBwaW5ncyI6Ijs7Ozs7QUFDbUM7QUFDVztBQUU5QyxNQUFNQSxZQUFZO0lBQ2hCLE1BQU1DLGFBQWFDLGFBQWFDLE9BQU8sQ0FBQyxXQUFXLHdCQUF3QjtJQUMzRSwyQkFBMkI7SUFDM0IsTUFBTUMsYUFBYUMsS0FBS0MsS0FBSyxDQUFDTCxhQUFhLDREQUE0RDtJQUV2RyxNQUFNTSxlQUFlLENBQUNDO1FBQ3BCQSxFQUFFQyxjQUFjO1FBQ2hCLE1BQU1DLG1CQUFtQkYsRUFBRUcsTUFBTSxDQUFDQyxTQUFTLENBQUNDLEtBQUs7UUFDakQsTUFBTUMsY0FBYztZQUNsQixHQUFHVixXQUFXVyxJQUFJO1lBQ2xCQyxrQkFBa0JOO1FBQ3BCO1FBQ0EsTUFBTU8sb0JBQW9CO1lBQUUsR0FBR2IsVUFBVTtZQUFFVyxNQUFNRDtRQUFZO1FBQzdEWixhQUFhZ0IsT0FBTyxDQUFDLFVBQVViLEtBQUtjLFNBQVMsQ0FBQ0Y7UUFDOUMsbURBQW1EO1FBQ25ERyxPQUFPQyxRQUFRLENBQUNDLE1BQU0sSUFBSSwrQ0FBK0M7SUFDM0U7SUFFQSxxQkFDRSw4REFBQ0M7OzBCQUNDLDhEQUFDQztnQkFBSUMsV0FBVTswQkFDYiw0RUFBQ0M7b0JBQUtDLFVBQVVwQjs7c0NBQ2QsOERBQUNxQjs0QkFBTUMsU0FBUTtzQ0FBWTs7Ozs7O3NDQUMzQiw4REFBQ0M7NEJBQ0NDLE1BQUs7NEJBQ0xDLElBQUc7NEJBQ0hDLGFBQVk7Ozs7OztzQ0FFZCw4REFBQ0M7NEJBQU9ILE1BQUs7c0NBQVM7Ozs7Ozs7Ozs7Ozs7Ozs7OzBCQUcxQiw4REFBQ1A7Z0JBQUlDLFdBQVU7MEJBQ2IsNEVBQUNVO29CQUNDQyxNQUFLLFFBQVEsOEJBQThCOztvQkFDM0NYLFdBQVU7OEJBQ1g7Ozs7Ozs7Ozs7Ozs7Ozs7O0FBTVQ7S0F6Q016QjtBQTJDTiwrREFBZUEsU0FBU0EsRUFBQyIsInNvdXJjZXMiOlsid2VicGFjazovL19OX0UvLi9zcmMvYXBwL2RpcmVjY2lvbi9wYWdlLmpzP2EwNTgiXSwic291cmNlc0NvbnRlbnQiOlsiXCJ1c2UgY2xpZW50XCIgLy92YSBjb24gZG9ibGUgY29taWxsYVxyXG5pbXBvcnQgXCIuLi8uLi9zdHlsZXMvZGlyZWNjaW9uLmNzc1wiXHJcbmltcG9ydCBcImJvb3RzdHJhcC9kaXN0L2Nzcy9ib290c3RyYXAubWluLmNzc1wiOyBcclxuXHJcbmNvbnN0IERpcmVjY2lvbiA9ICgpID0+IHtcclxuICBjb25zdCBzdG9yZWREYXRhID0gbG9jYWxTdG9yYWdlLmdldEl0ZW0oJ3RpZW5kYScpOyAvLyBPYnRpZW5lIGRhdG9zIGxvY2FsZXNcclxuICAvLyBjb25zb2xlLmxvZyhzdG9yZWREYXRhKTtcclxuICBjb25zdCBkYXRhT2JqZWN0ID0gSlNPTi5wYXJzZShzdG9yZWREYXRhKTsgLy8gQ29udmllcnRlIGEgb2JqZXRvIEphdmFTY3JpcHQgcGFyYSBhY2NlZGVyIGEgbGEgcHJvcGllZGFkXHJcblxyXG4gIGNvbnN0IGhhbmRsZVN1Ym1pdCA9IChlKSA9PiB7XHJcbiAgICBlLnByZXZlbnREZWZhdWx0KCk7XHJcbiAgICBjb25zdCB1cGRhdGVkRGlyZWN0aW9uID0gZS50YXJnZXQuZGlyZWNjaW9uLnZhbHVlO1xyXG4gICAgY29uc3QgdXBkYXRlZENhcnQgPSB7XHJcbiAgICAgIC4uLmRhdGFPYmplY3QuY2FydCxcclxuICAgICAgZGlyZWNjaW9uRW50cmVnYTogdXBkYXRlZERpcmVjdGlvbiBcclxuICAgIH07XHJcbiAgICBjb25zdCB1cGRhdGVkRGF0YU9iamVjdCA9IHsgLi4uZGF0YU9iamVjdCwgY2FydDogdXBkYXRlZENhcnQgfTtcclxuICAgIGxvY2FsU3RvcmFnZS5zZXRJdGVtKCd0aWVuZGEnLCBKU09OLnN0cmluZ2lmeSh1cGRhdGVkRGF0YU9iamVjdCkpO1xyXG4gICAgLy8gQWN0dWFsaXphciBlbCBlc3RhZG8gbyByZWRpcmlnaXIgc2kgZXMgbmVjZXNhcmlvXHJcbiAgICB3aW5kb3cubG9jYXRpb24ucmVsb2FkKCk7IC8vIFJlY2FyZ2FyIGxhIHDDoWdpbmEgcGFyYSByZWZsZWphciBsb3MgY2FtYmlvc1xyXG4gIH07XHJcblxyXG4gIHJldHVybiAoXHJcbiAgICA8YXJ0aWNsZT5cclxuICAgICAgPGRpdiBjbGFzc05hbWU9XCJmb3JtX2RpcmVjY2lvblwiPlxyXG4gICAgICAgIDxmb3JtIG9uU3VibWl0PXtoYW5kbGVTdWJtaXR9PlxyXG4gICAgICAgICAgPGxhYmVsIGh0bWxGb3I9XCJkaXJlY2Npb25cIj5EaXJlY2Npw7NuIGRlIGVudHJlZ2E6PC9sYWJlbD5cclxuICAgICAgICAgIDxpbnB1dFxyXG4gICAgICAgICAgICB0eXBlPVwidGV4dFwiXHJcbiAgICAgICAgICAgIGlkPVwiZGlyZWNjaW9uXCJcclxuICAgICAgICAgICAgcGxhY2Vob2xkZXI9XCJJbmdyZXNhIHR1IGRpcmVjY2nDs25cIlxyXG4gICAgICAgICAgLz5cclxuICAgICAgICAgIDxidXR0b24gdHlwZT1cInN1Ym1pdFwiPkFzaWduYXI8L2J1dHRvbj5cclxuICAgICAgICA8L2Zvcm0+XHJcbiAgICAgIDwvZGl2PlxyXG4gICAgICA8ZGl2IGNsYXNzTmFtZT1cImNhcnRfYm94XCI+XHJcbiAgICAgICAgPGFcclxuICAgICAgICAgIGhyZWY9XCIvcGFnb1wiIC8vIFVSTCBhIGxhIHDDoWdpbmEgZGUgY2hlY2tvdXRcclxuICAgICAgICAgIGNsYXNzTmFtZT1cImJ0biBidG4taW5mbyBtdC0zXCJcclxuICAgICAgICA+XHJcbiAgICAgICAgICBDb250aW51YXIgY29uIGxhIGNvbXByYVxyXG4gICAgICAgIDwvYT5cclxuICAgICAgPC9kaXY+XHJcbiAgICA8L2FydGljbGU+XHJcbiAgKTtcclxufTtcclxuXHJcbmV4cG9ydCBkZWZhdWx0IERpcmVjY2lvbjsiXSwibmFtZXMiOlsiRGlyZWNjaW9uIiwic3RvcmVkRGF0YSIsImxvY2FsU3RvcmFnZSIsImdldEl0ZW0iLCJkYXRhT2JqZWN0IiwiSlNPTiIsInBhcnNlIiwiaGFuZGxlU3VibWl0IiwiZSIsInByZXZlbnREZWZhdWx0IiwidXBkYXRlZERpcmVjdGlvbiIsInRhcmdldCIsImRpcmVjY2lvbiIsInZhbHVlIiwidXBkYXRlZENhcnQiLCJjYXJ0IiwiZGlyZWNjaW9uRW50cmVnYSIsInVwZGF0ZWREYXRhT2JqZWN0Iiwic2V0SXRlbSIsInN0cmluZ2lmeSIsIndpbmRvdyIsImxvY2F0aW9uIiwicmVsb2FkIiwiYXJ0aWNsZSIsImRpdiIsImNsYXNzTmFtZSIsImZvcm0iLCJvblN1Ym1pdCIsImxhYmVsIiwiaHRtbEZvciIsImlucHV0IiwidHlwZSIsImlkIiwicGxhY2Vob2xkZXIiLCJidXR0b24iLCJhIiwiaHJlZiJdLCJzb3VyY2VSb290IjoiIn0=\n//# sourceURL=webpack-internal:///(app-pages-browser)/./src/app/direccion/page.js\n"));

/***/ })

});