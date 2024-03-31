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

eval(__webpack_require__.ts("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react/jsx-dev-runtime */ \"(app-pages-browser)/./node_modules/next/dist/compiled/react/jsx-dev-runtime.js\");\n/* harmony import */ var _styles_direccion_css__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../../styles/direccion.css */ \"(app-pages-browser)/./src/styles/direccion.css\");\n/* harmony import */ var bootstrap_dist_css_bootstrap_min_css__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! bootstrap/dist/css/bootstrap.min.css */ \"(app-pages-browser)/./node_modules/bootstrap/dist/css/bootstrap.min.css\");\n/* harmony import */ var _components_Navbar__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../../components/Navbar */ \"(app-pages-browser)/./src/components/Navbar.js\");\n/* __next_internal_client_entry_do_not_use__ default auto */ \n\n\n\nconst Direccion = ()=>{\n    const storedData = localStorage.getItem(\"tienda\"); // Obtiene datos locales\n    // console.log(storedData);\n    const dataObject = JSON.parse(storedData); // Convierte a objeto JavaScript para acceder a la propiedad\n    const handleSubmit = (e)=>{\n        e.preventDefault();\n        const updatedDirection = e.target.direccion.value;\n        const updatedCart = {\n            ...dataObject.cart,\n            direccionEntrega: updatedDirection\n        };\n        const updatedDataObject = {\n            ...dataObject,\n            cart: updatedCart\n        };\n        localStorage.setItem(\"tienda\", JSON.stringify(updatedDataObject));\n        // Actualizar el estado o redirigir si es necesario\n        window.location.reload(); // Recargar la página para reflejar los cambios\n    };\n    return /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"article\", {\n        children: [\n            /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"div\", {\n                children: /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(_components_Navbar__WEBPACK_IMPORTED_MODULE_3__[\"default\"], {\n                    size: dataObject.cart.productos.length\n                }, void 0, false, {\n                    fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\direccion\\\\page.js\",\n                    lineNumber: 27,\n                    columnNumber: 9\n                }, undefined)\n            }, void 0, false, {\n                fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\direccion\\\\page.js\",\n                lineNumber: 26,\n                columnNumber: 7\n            }, undefined),\n            /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"div\", {\n                className: \"form_direccion\",\n                children: [\n                    /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"form\", {\n                        onSubmit: handleSubmit,\n                        children: [\n                            /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"label\", {\n                                htmlFor: \"direccion\",\n                                children: \"Direcci\\xf3n de entrega:\"\n                            }, void 0, false, {\n                                fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\direccion\\\\page.js\",\n                                lineNumber: 31,\n                                columnNumber: 11\n                            }, undefined),\n                            /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"input\", {\n                                type: \"text\",\n                                id: \"direccion\",\n                                placeholder: \"Ingresa tu direcci\\xf3n\"\n                            }, void 0, false, {\n                                fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\direccion\\\\page.js\",\n                                lineNumber: 32,\n                                columnNumber: 11\n                            }, undefined),\n                            /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"button\", {\n                                className: \"btnAsignar\",\n                                type: \"submit\",\n                                children: \"Asignar\"\n                            }, void 0, false, {\n                                fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\direccion\\\\page.js\",\n                                lineNumber: 37,\n                                columnNumber: 11\n                            }, undefined)\n                        ]\n                    }, void 0, true, {\n                        fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\direccion\\\\page.js\",\n                        lineNumber: 30,\n                        columnNumber: 9\n                    }, undefined),\n                    /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"div\", {\n                        className: \"cart_box\",\n                        children: /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"a\", {\n                            href: \"/pago\" // URL a la página de checkout\n                            ,\n                            className: \"btn btn-info mt-3\",\n                            children: \"Continuar con la compra\"\n                        }, void 0, false, {\n                            fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\direccion\\\\page.js\",\n                            lineNumber: 40,\n                            columnNumber: 9\n                        }, undefined)\n                    }, void 0, false, {\n                        fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\direccion\\\\page.js\",\n                        lineNumber: 39,\n                        columnNumber: 9\n                    }, undefined)\n                ]\n            }, void 0, true, {\n                fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\direccion\\\\page.js\",\n                lineNumber: 29,\n                columnNumber: 7\n            }, undefined)\n        ]\n    }, void 0, true, {\n        fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\direccion\\\\page.js\",\n        lineNumber: 25,\n        columnNumber: 5\n    }, undefined);\n};\n_c = Direccion;\n/* harmony default export */ __webpack_exports__[\"default\"] = (Direccion);\nvar _c;\n$RefreshReg$(_c, \"Direccion\");\n\n\n;\n    // Wrapped in an IIFE to avoid polluting the global scope\n    ;\n    (function () {\n        var _a, _b;\n        // Legacy CSS implementations will `eval` browser code in a Node.js context\n        // to extract CSS. For backwards compatibility, we need to check we're in a\n        // browser context before continuing.\n        if (typeof self !== 'undefined' &&\n            // AMP / No-JS mode does not inject these helpers:\n            '$RefreshHelpers$' in self) {\n            // @ts-ignore __webpack_module__ is global\n            var currentExports = module.exports;\n            // @ts-ignore __webpack_module__ is global\n            var prevSignature = (_b = (_a = module.hot.data) === null || _a === void 0 ? void 0 : _a.prevSignature) !== null && _b !== void 0 ? _b : null;\n            // This cannot happen in MainTemplate because the exports mismatch between\n            // templating and execution.\n            self.$RefreshHelpers$.registerExportsForReactRefresh(currentExports, module.id);\n            // A module can be accepted automatically based on its exports, e.g. when\n            // it is a Refresh Boundary.\n            if (self.$RefreshHelpers$.isReactRefreshBoundary(currentExports)) {\n                // Save the previous exports signature on update so we can compare the boundary\n                // signatures. We avoid saving exports themselves since it causes memory leaks (https://github.com/vercel/next.js/pull/53797)\n                module.hot.dispose(function (data) {\n                    data.prevSignature =\n                        self.$RefreshHelpers$.getRefreshBoundarySignature(currentExports);\n                });\n                // Unconditionally accept an update to this module, we'll check if it's\n                // still a Refresh Boundary later.\n                // @ts-ignore importMeta is replaced in the loader\n                module.hot.accept();\n                // This field is set when the previous version of this module was a\n                // Refresh Boundary, letting us know we need to check for invalidation or\n                // enqueue an update.\n                if (prevSignature !== null) {\n                    // A boundary can become ineligible if its exports are incompatible\n                    // with the previous exports.\n                    //\n                    // For example, if you add/remove/change exports, we'll want to\n                    // re-execute the importing modules, and force those components to\n                    // re-render. Similarly, if you convert a class component to a\n                    // function, we want to invalidate the boundary.\n                    if (self.$RefreshHelpers$.shouldInvalidateReactRefreshBoundary(prevSignature, self.$RefreshHelpers$.getRefreshBoundarySignature(currentExports))) {\n                        module.hot.invalidate();\n                    }\n                    else {\n                        self.$RefreshHelpers$.scheduleUpdate();\n                    }\n                }\n            }\n            else {\n                // Since we just executed the code for the module, it's possible that the\n                // new exports made it ineligible for being a boundary.\n                // We only care about the case when we were _previously_ a boundary,\n                // because we already accepted this update (accidental side effect).\n                var isNoLongerABoundary = prevSignature !== null;\n                if (isNoLongerABoundary) {\n                    module.hot.invalidate();\n                }\n            }\n        }\n    })();\n//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiKGFwcC1wYWdlcy1icm93c2VyKS8uL3NyYy9hcHAvZGlyZWNjaW9uL3BhZ2UuanMiLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQ21DO0FBQ1c7QUFDRDtBQUU3QyxNQUFNQyxZQUFZO0lBQ2hCLE1BQU1DLGFBQWFDLGFBQWFDLE9BQU8sQ0FBQyxXQUFXLHdCQUF3QjtJQUMzRSwyQkFBMkI7SUFDM0IsTUFBTUMsYUFBYUMsS0FBS0MsS0FBSyxDQUFDTCxhQUFhLDREQUE0RDtJQUV2RyxNQUFNTSxlQUFlLENBQUNDO1FBQ3BCQSxFQUFFQyxjQUFjO1FBQ2hCLE1BQU1DLG1CQUFtQkYsRUFBRUcsTUFBTSxDQUFDQyxTQUFTLENBQUNDLEtBQUs7UUFDakQsTUFBTUMsY0FBYztZQUNsQixHQUFHVixXQUFXVyxJQUFJO1lBQ2xCQyxrQkFBa0JOO1FBQ3BCO1FBQ0EsTUFBTU8sb0JBQW9CO1lBQUUsR0FBR2IsVUFBVTtZQUFFVyxNQUFNRDtRQUFZO1FBQzdEWixhQUFhZ0IsT0FBTyxDQUFDLFVBQVViLEtBQUtjLFNBQVMsQ0FBQ0Y7UUFDOUMsbURBQW1EO1FBQ25ERyxPQUFPQyxRQUFRLENBQUNDLE1BQU0sSUFBSSwrQ0FBK0M7SUFDM0U7SUFFQSxxQkFDRSw4REFBQ0M7OzBCQUNDLDhEQUFDQzswQkFDQyw0RUFBQ3pCLDBEQUFNQTtvQkFBQzBCLE1BQU1yQixXQUFXVyxJQUFJLENBQUNXLFNBQVMsQ0FBQ0MsTUFBTTs7Ozs7Ozs7Ozs7MEJBRWhELDhEQUFDSDtnQkFBSUksV0FBVTs7a0NBQ2IsOERBQUNDO3dCQUFLQyxVQUFVdkI7OzBDQUNkLDhEQUFDd0I7Z0NBQU1DLFNBQVE7MENBQVk7Ozs7OzswQ0FDM0IsOERBQUNDO2dDQUNDQyxNQUFLO2dDQUNMQyxJQUFHO2dDQUNIQyxhQUFZOzs7Ozs7MENBRWQsOERBQUNDO2dDQUFPVCxXQUFVO2dDQUFhTSxNQUFLOzBDQUFTOzs7Ozs7Ozs7Ozs7a0NBRS9DLDhEQUFDVjt3QkFBSUksV0FBVTtrQ0FDZiw0RUFBQ1U7NEJBQ0NDLE1BQUssUUFBUSw4QkFBOEI7OzRCQUMzQ1gsV0FBVTtzQ0FDWDs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7QUFRVDtLQTdDTTVCO0FBK0NOLCtEQUFlQSxTQUFTQSxFQUFDIiwic291cmNlcyI6WyJ3ZWJwYWNrOi8vX05fRS8uL3NyYy9hcHAvZGlyZWNjaW9uL3BhZ2UuanM/YTA1OCJdLCJzb3VyY2VzQ29udGVudCI6WyJcInVzZSBjbGllbnRcIiAvL3ZhIGNvbiBkb2JsZSBjb21pbGxhXHJcbmltcG9ydCBcIi4uLy4uL3N0eWxlcy9kaXJlY2Npb24uY3NzXCJcclxuaW1wb3J0IFwiYm9vdHN0cmFwL2Rpc3QvY3NzL2Jvb3RzdHJhcC5taW4uY3NzXCI7IFxyXG5pbXBvcnQgTmF2YmFyIGZyb20gJy4uLy4uL2NvbXBvbmVudHMvTmF2YmFyJztcclxuXHJcbmNvbnN0IERpcmVjY2lvbiA9ICgpID0+IHtcclxuICBjb25zdCBzdG9yZWREYXRhID0gbG9jYWxTdG9yYWdlLmdldEl0ZW0oJ3RpZW5kYScpOyAvLyBPYnRpZW5lIGRhdG9zIGxvY2FsZXNcclxuICAvLyBjb25zb2xlLmxvZyhzdG9yZWREYXRhKTtcclxuICBjb25zdCBkYXRhT2JqZWN0ID0gSlNPTi5wYXJzZShzdG9yZWREYXRhKTsgLy8gQ29udmllcnRlIGEgb2JqZXRvIEphdmFTY3JpcHQgcGFyYSBhY2NlZGVyIGEgbGEgcHJvcGllZGFkXHJcblxyXG4gIGNvbnN0IGhhbmRsZVN1Ym1pdCA9IChlKSA9PiB7XHJcbiAgICBlLnByZXZlbnREZWZhdWx0KCk7XHJcbiAgICBjb25zdCB1cGRhdGVkRGlyZWN0aW9uID0gZS50YXJnZXQuZGlyZWNjaW9uLnZhbHVlO1xyXG4gICAgY29uc3QgdXBkYXRlZENhcnQgPSB7XHJcbiAgICAgIC4uLmRhdGFPYmplY3QuY2FydCxcclxuICAgICAgZGlyZWNjaW9uRW50cmVnYTogdXBkYXRlZERpcmVjdGlvbiBcclxuICAgIH07XHJcbiAgICBjb25zdCB1cGRhdGVkRGF0YU9iamVjdCA9IHsgLi4uZGF0YU9iamVjdCwgY2FydDogdXBkYXRlZENhcnQgfTtcclxuICAgIGxvY2FsU3RvcmFnZS5zZXRJdGVtKCd0aWVuZGEnLCBKU09OLnN0cmluZ2lmeSh1cGRhdGVkRGF0YU9iamVjdCkpO1xyXG4gICAgLy8gQWN0dWFsaXphciBlbCBlc3RhZG8gbyByZWRpcmlnaXIgc2kgZXMgbmVjZXNhcmlvXHJcbiAgICB3aW5kb3cubG9jYXRpb24ucmVsb2FkKCk7IC8vIFJlY2FyZ2FyIGxhIHDDoWdpbmEgcGFyYSByZWZsZWphciBsb3MgY2FtYmlvc1xyXG4gIH07XHJcblxyXG4gIHJldHVybiAoXHJcbiAgICA8YXJ0aWNsZT5cclxuICAgICAgPGRpdj5cclxuICAgICAgICA8TmF2YmFyIHNpemU9e2RhdGFPYmplY3QuY2FydC5wcm9kdWN0b3MubGVuZ3RofS8+XHJcbiAgICAgIDwvZGl2PlxyXG4gICAgICA8ZGl2IGNsYXNzTmFtZT1cImZvcm1fZGlyZWNjaW9uXCI+XHJcbiAgICAgICAgPGZvcm0gb25TdWJtaXQ9e2hhbmRsZVN1Ym1pdH0+XHJcbiAgICAgICAgICA8bGFiZWwgaHRtbEZvcj1cImRpcmVjY2lvblwiPkRpcmVjY2nDs24gZGUgZW50cmVnYTo8L2xhYmVsPlxyXG4gICAgICAgICAgPGlucHV0XHJcbiAgICAgICAgICAgIHR5cGU9XCJ0ZXh0XCJcclxuICAgICAgICAgICAgaWQ9XCJkaXJlY2Npb25cIlxyXG4gICAgICAgICAgICBwbGFjZWhvbGRlcj1cIkluZ3Jlc2EgdHUgZGlyZWNjacOzblwiXHJcbiAgICAgICAgICAvPlxyXG4gICAgICAgICAgPGJ1dHRvbiBjbGFzc05hbWU9XCJidG5Bc2lnbmFyXCIgdHlwZT1cInN1Ym1pdFwiPkFzaWduYXI8L2J1dHRvbj5cclxuICAgICAgICA8L2Zvcm0+XHJcbiAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJjYXJ0X2JveFwiPlxyXG4gICAgICAgIDxhXHJcbiAgICAgICAgICBocmVmPVwiL3BhZ29cIiAvLyBVUkwgYSBsYSBww6FnaW5hIGRlIGNoZWNrb3V0XHJcbiAgICAgICAgICBjbGFzc05hbWU9XCJidG4gYnRuLWluZm8gbXQtM1wiXHJcbiAgICAgICAgPlxyXG4gICAgICAgICAgQ29udGludWFyIGNvbiBsYSBjb21wcmFcclxuICAgICAgICA8L2E+XHJcbiAgICAgIDwvZGl2PiBcclxuICAgICAgPC9kaXY+XHJcbiAgICA8L2FydGljbGU+XHJcbiAgICBcclxuICApO1xyXG59O1xyXG5cclxuZXhwb3J0IGRlZmF1bHQgRGlyZWNjaW9uOyJdLCJuYW1lcyI6WyJOYXZiYXIiLCJEaXJlY2Npb24iLCJzdG9yZWREYXRhIiwibG9jYWxTdG9yYWdlIiwiZ2V0SXRlbSIsImRhdGFPYmplY3QiLCJKU09OIiwicGFyc2UiLCJoYW5kbGVTdWJtaXQiLCJlIiwicHJldmVudERlZmF1bHQiLCJ1cGRhdGVkRGlyZWN0aW9uIiwidGFyZ2V0IiwiZGlyZWNjaW9uIiwidmFsdWUiLCJ1cGRhdGVkQ2FydCIsImNhcnQiLCJkaXJlY2Npb25FbnRyZWdhIiwidXBkYXRlZERhdGFPYmplY3QiLCJzZXRJdGVtIiwic3RyaW5naWZ5Iiwid2luZG93IiwibG9jYXRpb24iLCJyZWxvYWQiLCJhcnRpY2xlIiwiZGl2Iiwic2l6ZSIsInByb2R1Y3RvcyIsImxlbmd0aCIsImNsYXNzTmFtZSIsImZvcm0iLCJvblN1Ym1pdCIsImxhYmVsIiwiaHRtbEZvciIsImlucHV0IiwidHlwZSIsImlkIiwicGxhY2Vob2xkZXIiLCJidXR0b24iLCJhIiwiaHJlZiJdLCJzb3VyY2VSb290IjoiIn0=\n//# sourceURL=webpack-internal:///(app-pages-browser)/./src/app/direccion/page.js\n"));

/***/ })

});