"use strict";
/*
 * ATTENTION: An "eval-source-map" devtool has been used.
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file with attached SourceMaps in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
self["webpackHotUpdate_N_E"]("app/page",{

/***/ "(app-pages-browser)/./src/app/page.js":
/*!*************************!*\
  !*** ./src/app/page.js ***!
  \*************************/
/***/ (function(module, __webpack_exports__, __webpack_require__) {

eval(__webpack_require__.ts("__webpack_require__.r(__webpack_exports__);\n/* harmony export */ __webpack_require__.d(__webpack_exports__, {\n/* harmony export */   \"default\": function() { return /* binding */ Home; }\n/* harmony export */ });\n/* harmony import */ var react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react/jsx-dev-runtime */ \"(app-pages-browser)/./node_modules/next/dist/compiled/react/jsx-dev-runtime.js\");\n/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react */ \"(app-pages-browser)/./node_modules/next/dist/compiled/react/index.js\");\n/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_1__);\n/* harmony import */ var bootstrap_dist_css_bootstrap_min_css__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! bootstrap/dist/css/bootstrap.min.css */ \"(app-pages-browser)/./node_modules/bootstrap/dist/css/bootstrap.min.css\");\n/* harmony import */ var _components_Navbar__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ../components/Navbar */ \"(app-pages-browser)/./src/components/Navbar.js\");\n/* harmony import */ var _components_StorePage__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! ../components/StorePage */ \"(app-pages-browser)/./src/components/StorePage.js\");\n/* __next_internal_client_entry_do_not_use__ default auto */ \nvar _s = $RefreshSig$();\n\n\n\n\nfunction Home() {\n    _s();\n    const initialState = {\n        impVentas: 13,\n        cart: {\n            productos: [],\n            subtotal: 0,\n            total: 0,\n            direccionEntrega: \"\",\n            metodosPago: \"\",\n            necesitaVerificacion: false\n        }\n    };\n    // Obtener el estado de tienda desde localStorage o usar el estado inicial\n    const [tienda, setTienda] = (0,react__WEBPACK_IMPORTED_MODULE_1__.useState)(()=>{\n        const storedTienda = localStorage.getItem(\"tienda\");\n        return storedTienda ? JSON.parse(storedTienda) : initialState;\n    });\n    // Guardar el estado de tienda en localStorage cuando cambie\n    (0,react__WEBPACK_IMPORTED_MODULE_1__.useEffect)(()=>{\n        localStorage.setItem(\"tienda\", JSON.stringify(tienda));\n    }, [\n        tienda\n    ]);\n    const handleClickAdd = (item)=>{\n        const isPresent = tienda.cart.productos.some((producto)=>producto.id === item.id);\n        if (!isPresent) {\n            // Si el item no está presente, actualizar el estado\n            const nuevosProductos = [\n                ...tienda.cart.productos,\n                item\n            ];\n            //se encarga de sumar los precios en los productos\n            const nuevoSubtotal = nuevosProductos.reduce((total, producto)=>total + producto.precio, 0);\n            const nuevoTotal = nuevoSubtotal * (1 + tienda.impVentas / 100);\n            const direccionS = \" \";\n            const necesitaVerificacionS = false;\n            setTienda({\n                ...tienda,\n                cart: {\n                    ...tienda.cart,\n                    productos: nuevosProductos,\n                    subtotal: nuevoSubtotal,\n                    total: nuevoTotal,\n                    direccionEntrega: direccionS,\n                    necesitaVerificacion: necesitaVerificacionS\n                }\n            });\n        }\n    };\n    return /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"div\", {\n        children: [\n            /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"div\", {\n                children: /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(_components_Navbar__WEBPACK_IMPORTED_MODULE_3__[\"default\"], {\n                    size: tienda.cart.productos.length\n                }, void 0, false, {\n                    fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\page.js\",\n                    lineNumber: 56,\n                    columnNumber: 9\n                }, this)\n            }, void 0, false, {\n                fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\page.js\",\n                lineNumber: 55,\n                columnNumber: 7\n            }, this),\n            /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"main\", {\n                children: /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"div\", {\n                    className: \"container\",\n                    children: /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(\"div\", {\n                        children: [\n                            /*#__PURE__*/ (0,react_jsx_dev_runtime__WEBPACK_IMPORTED_MODULE_0__.jsxDEV)(_components_StorePage__WEBPACK_IMPORTED_MODULE_4__[\"default\"], {\n                                handleClickAdd: handleClickAdd\n                            }, void 0, false, {\n                                fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\page.js\",\n                                lineNumber: 63,\n                                columnNumber: 11\n                            }, this),\n                            \" \"\n                        ]\n                    }, void 0, true, {\n                        fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\page.js\",\n                        lineNumber: 62,\n                        columnNumber: 9\n                    }, this)\n                }, void 0, false, {\n                    fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\page.js\",\n                    lineNumber: 61,\n                    columnNumber: 7\n                }, this)\n            }, void 0, false, {\n                fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\page.js\",\n                lineNumber: 60,\n                columnNumber: 7\n            }, this)\n        ]\n    }, void 0, true, {\n        fileName: \"C:\\\\Users\\\\Usuario\\\\Desktop\\\\Local storage\\\\local\\\\src\\\\app\\\\page.js\",\n        lineNumber: 53,\n        columnNumber: 5\n    }, this);\n}\n_s(Home, \"ohYcm8aBiZE1U3jtsaaQUUa/uNU=\");\n_c = Home;\nvar _c;\n$RefreshReg$(_c, \"Home\");\n\n\n;\n    // Wrapped in an IIFE to avoid polluting the global scope\n    ;\n    (function () {\n        var _a, _b;\n        // Legacy CSS implementations will `eval` browser code in a Node.js context\n        // to extract CSS. For backwards compatibility, we need to check we're in a\n        // browser context before continuing.\n        if (typeof self !== 'undefined' &&\n            // AMP / No-JS mode does not inject these helpers:\n            '$RefreshHelpers$' in self) {\n            // @ts-ignore __webpack_module__ is global\n            var currentExports = module.exports;\n            // @ts-ignore __webpack_module__ is global\n            var prevSignature = (_b = (_a = module.hot.data) === null || _a === void 0 ? void 0 : _a.prevSignature) !== null && _b !== void 0 ? _b : null;\n            // This cannot happen in MainTemplate because the exports mismatch between\n            // templating and execution.\n            self.$RefreshHelpers$.registerExportsForReactRefresh(currentExports, module.id);\n            // A module can be accepted automatically based on its exports, e.g. when\n            // it is a Refresh Boundary.\n            if (self.$RefreshHelpers$.isReactRefreshBoundary(currentExports)) {\n                // Save the previous exports signature on update so we can compare the boundary\n                // signatures. We avoid saving exports themselves since it causes memory leaks (https://github.com/vercel/next.js/pull/53797)\n                module.hot.dispose(function (data) {\n                    data.prevSignature =\n                        self.$RefreshHelpers$.getRefreshBoundarySignature(currentExports);\n                });\n                // Unconditionally accept an update to this module, we'll check if it's\n                // still a Refresh Boundary later.\n                // @ts-ignore importMeta is replaced in the loader\n                module.hot.accept();\n                // This field is set when the previous version of this module was a\n                // Refresh Boundary, letting us know we need to check for invalidation or\n                // enqueue an update.\n                if (prevSignature !== null) {\n                    // A boundary can become ineligible if its exports are incompatible\n                    // with the previous exports.\n                    //\n                    // For example, if you add/remove/change exports, we'll want to\n                    // re-execute the importing modules, and force those components to\n                    // re-render. Similarly, if you convert a class component to a\n                    // function, we want to invalidate the boundary.\n                    if (self.$RefreshHelpers$.shouldInvalidateReactRefreshBoundary(prevSignature, self.$RefreshHelpers$.getRefreshBoundarySignature(currentExports))) {\n                        module.hot.invalidate();\n                    }\n                    else {\n                        self.$RefreshHelpers$.scheduleUpdate();\n                    }\n                }\n            }\n            else {\n                // Since we just executed the code for the module, it's possible that the\n                // new exports made it ineligible for being a boundary.\n                // We only care about the case when we were _previously_ a boundary,\n                // because we already accepted this update (accidental side effect).\n                var isNoLongerABoundary = prevSignature !== null;\n                if (isNoLongerABoundary) {\n                    module.hot.invalidate();\n                }\n            }\n        }\n    })();\n//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiKGFwcC1wYWdlcy1icm93c2VyKS8uL3NyYy9hcHAvcGFnZS5qcyIsIm1hcHBpbmdzIjoiOzs7Ozs7Ozs7Ozs7QUFDbUQ7QUFDTDtBQUNKO0FBQ007QUFHakMsU0FBU0s7O0lBRXRCLE1BQU1DLGVBQWU7UUFDbkJDLFdBQVc7UUFDWEMsTUFBTTtZQUFFQyxXQUFXLEVBQUU7WUFBRUMsVUFBVTtZQUFHQyxPQUFPO1lBQUdDLGtCQUFrQjtZQUFJQyxhQUFjO1lBQUlDLHNCQUFzQjtRQUFLO0lBQ25IO0lBRUEsMEVBQTBFO0lBQzFFLE1BQU0sQ0FBQ0MsUUFBUUMsVUFBVSxHQUFHZiwrQ0FBUUEsQ0FBQztRQUNuQyxNQUFNZ0IsZUFBZUMsYUFBYUMsT0FBTyxDQUFDO1FBQzFDLE9BQU9GLGVBQWVHLEtBQUtDLEtBQUssQ0FBQ0osZ0JBQWdCWDtJQUNuRDtJQUVBLDREQUE0RDtJQUM1REosZ0RBQVNBLENBQUM7UUFDUmdCLGFBQWFJLE9BQU8sQ0FBQyxVQUFVRixLQUFLRyxTQUFTLENBQUNSO0lBQ2hELEdBQUc7UUFBQ0E7S0FBTztJQUViLE1BQU1TLGlCQUFlLENBQUNDO1FBQ3BCLE1BQU1DLFlBQVlYLE9BQU9QLElBQUksQ0FBQ0MsU0FBUyxDQUFDa0IsSUFBSSxDQUFDQyxDQUFBQSxXQUFZQSxTQUFTQyxFQUFFLEtBQUtKLEtBQUtJLEVBQUU7UUFFaEYsSUFBSSxDQUFDSCxXQUFXO1lBQ2Qsb0RBQW9EO1lBQ3BELE1BQU1JLGtCQUFrQjttQkFBSWYsT0FBT1AsSUFBSSxDQUFDQyxTQUFTO2dCQUFFZ0I7YUFBSztZQUN4RCxrREFBa0Q7WUFDbEQsTUFBTU0sZ0JBQWdCRCxnQkFBZ0JFLE1BQU0sQ0FBQyxDQUFDckIsT0FBT2lCLFdBQWFqQixRQUFRaUIsU0FBU0ssTUFBTSxFQUFFO1lBQzNGLE1BQU1DLGFBQWFILGdCQUFpQixLQUFJaEIsT0FBT1IsU0FBUyxHQUFHLEdBQUU7WUFDN0QsTUFBTTRCLGFBQWE7WUFDbkIsTUFBTUMsd0JBQXdCO1lBRTlCcEIsVUFBVTtnQkFDUixHQUFHRCxNQUFNO2dCQUNUUCxNQUFNO29CQUNKLEdBQUdPLE9BQU9QLElBQUk7b0JBQ2RDLFdBQVdxQjtvQkFDWHBCLFVBQVVxQjtvQkFDVnBCLE9BQU91QjtvQkFDUHRCLGtCQUFrQnVCO29CQUNsQnJCLHNCQUFzQnNCO2dCQUN4QjtZQUNGO1FBQ0Y7SUFDRjtJQUVBLHFCQUNJLDhEQUFDQzs7MEJBRUMsOERBQUNBOzBCQUNDLDRFQUFDbEMsMERBQU1BO29CQUFDbUMsTUFBTXZCLE9BQU9QLElBQUksQ0FBQ0MsU0FBUyxDQUFDOEIsTUFBTTs7Ozs7Ozs7Ozs7MEJBSTVDLDhEQUFDQzswQkFDRCw0RUFBQ0g7b0JBQUlJLFdBQVU7OEJBQ2IsNEVBQUNKOzswQ0FDQyw4REFBQ2pDLDZEQUFTQTtnQ0FDVm9CLGdCQUFnQkE7Ozs7Ozs0QkFBaUI7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7O0FBUTNDO0dBaEV3Qm5CO0tBQUFBIiwic291cmNlcyI6WyJ3ZWJwYWNrOi8vX05fRS8uL3NyYy9hcHAvcGFnZS5qcz8yYjNkIl0sInNvdXJjZXNDb250ZW50IjpbIlwidXNlIGNsaWVudFwiIC8vdmEgY29uIGRvYmxlIGNvbWlsbGFcbmltcG9ydCBSZWFjdCwgeyB1c2VTdGF0ZSwgdXNlRWZmZWN0IH0gZnJvbSAncmVhY3QnO1xuaW1wb3J0IFwiYm9vdHN0cmFwL2Rpc3QvY3NzL2Jvb3RzdHJhcC5taW4uY3NzXCI7IFxuaW1wb3J0IE5hdmJhciBmcm9tICcuLi9jb21wb25lbnRzL05hdmJhcic7XG5pbXBvcnQgU3RvcmVQYWdlIGZyb20gJy4uL2NvbXBvbmVudHMvU3RvcmVQYWdlJztcblxuXG5leHBvcnQgZGVmYXVsdCBmdW5jdGlvbiBIb21lKCkge1xuXG4gIGNvbnN0IGluaXRpYWxTdGF0ZSA9IHtcbiAgICBpbXBWZW50YXM6IDEzLFxuICAgIGNhcnQ6IHsgcHJvZHVjdG9zOiBbXSwgc3VidG90YWw6IDAsIHRvdGFsOiAwLCBkaXJlY2Npb25FbnRyZWdhOiAnJywgbWV0b2Rvc1BhZ28gOiAnJywgbmVjZXNpdGFWZXJpZmljYWNpb246IGZhbHNlfSxcbiAgfTtcbiAgXG4gIC8vIE9idGVuZXIgZWwgZXN0YWRvIGRlIHRpZW5kYSBkZXNkZSBsb2NhbFN0b3JhZ2UgbyB1c2FyIGVsIGVzdGFkbyBpbmljaWFsXG4gIGNvbnN0IFt0aWVuZGEsIHNldFRpZW5kYV0gPSB1c2VTdGF0ZSgoKSA9PiB7XG4gICAgY29uc3Qgc3RvcmVkVGllbmRhID0gbG9jYWxTdG9yYWdlLmdldEl0ZW0oJ3RpZW5kYScpO1xuICAgIHJldHVybiBzdG9yZWRUaWVuZGEgPyBKU09OLnBhcnNlKHN0b3JlZFRpZW5kYSkgOiBpbml0aWFsU3RhdGU7XG4gIH0pO1xuXG4gIC8vIEd1YXJkYXIgZWwgZXN0YWRvIGRlIHRpZW5kYSBlbiBsb2NhbFN0b3JhZ2UgY3VhbmRvIGNhbWJpZVxuICB1c2VFZmZlY3QoKCkgPT4ge1xuICAgIGxvY2FsU3RvcmFnZS5zZXRJdGVtKCd0aWVuZGEnLCBKU09OLnN0cmluZ2lmeSh0aWVuZGEpKTtcbiAgfSwgW3RpZW5kYV0pO1xuXG5jb25zdCBoYW5kbGVDbGlja0FkZD0oaXRlbSk9PnsgLy8xLiBwb3IgbWVkaW8gZGUgZXN0YSBmdW5jaW9uIHB1ZWRvIGVudmlhciB1biBvYmpldG8gcG9yIGVsIGJvdG9uXG4gIGNvbnN0IGlzUHJlc2VudCA9IHRpZW5kYS5jYXJ0LnByb2R1Y3Rvcy5zb21lKHByb2R1Y3RvID0+IHByb2R1Y3RvLmlkID09PSBpdGVtLmlkKTtcblxuICBpZiAoIWlzUHJlc2VudCkge1xuICAgIC8vIFNpIGVsIGl0ZW0gbm8gZXN0w6EgcHJlc2VudGUsIGFjdHVhbGl6YXIgZWwgZXN0YWRvXG4gICAgY29uc3QgbnVldm9zUHJvZHVjdG9zID0gWy4uLnRpZW5kYS5jYXJ0LnByb2R1Y3RvcywgaXRlbV07XG4gICAgLy9zZSBlbmNhcmdhIGRlIHN1bWFyIGxvcyBwcmVjaW9zIGVuIGxvcyBwcm9kdWN0b3NcbiAgICBjb25zdCBudWV2b1N1YnRvdGFsID0gbnVldm9zUHJvZHVjdG9zLnJlZHVjZSgodG90YWwsIHByb2R1Y3RvKSA9PiB0b3RhbCArIHByb2R1Y3RvLnByZWNpbywgMCk7XG4gICAgY29uc3QgbnVldm9Ub3RhbCA9IG51ZXZvU3VidG90YWwgKiAoMSArIHRpZW5kYS5pbXBWZW50YXMgLyAxMDApO1xuICAgIGNvbnN0IGRpcmVjY2lvblMgPSAnICc7XG4gICAgY29uc3QgbmVjZXNpdGFWZXJpZmljYWNpb25TID0gZmFsc2VcblxuICAgIHNldFRpZW5kYSh7XG4gICAgICAuLi50aWVuZGEsXG4gICAgICBjYXJ0OiB7XG4gICAgICAgIC4uLnRpZW5kYS5jYXJ0LFxuICAgICAgICBwcm9kdWN0b3M6IG51ZXZvc1Byb2R1Y3RvcyxcbiAgICAgICAgc3VidG90YWw6IG51ZXZvU3VidG90YWwsXG4gICAgICAgIHRvdGFsOiBudWV2b1RvdGFsLFxuICAgICAgICBkaXJlY2Npb25FbnRyZWdhOiBkaXJlY2Npb25TLFxuICAgICAgICBuZWNlc2l0YVZlcmlmaWNhY2lvbjogbmVjZXNpdGFWZXJpZmljYWNpb25TXG4gICAgICB9XG4gICAgfSk7XG4gIH1cbn07XG5cbnJldHVybiAoIFxuICAgIDxkaXY+XG4gICAgey8qaGVhZGVyKi99XG4gICAgICA8ZGl2PlxuICAgICAgICA8TmF2YmFyIHNpemU9e3RpZW5kYS5jYXJ0LnByb2R1Y3Rvcy5sZW5ndGh9Lz5cbiAgICAgIDwvZGl2PlxuXG4gICAgey8qcGFnaW5hIHByaW5jaXBhbCovfVxuICAgICAgPG1haW4+XG4gICAgICA8ZGl2IGNsYXNzTmFtZT1cImNvbnRhaW5lclwiID5cbiAgICAgICAgPGRpdj5cbiAgICAgICAgICA8U3RvcmVQYWdlIFxuICAgICAgICAgIGhhbmRsZUNsaWNrQWRkPXtoYW5kbGVDbGlja0FkZH0vPiB7LypFbCBoYW5kbGUgY2xpY2sgYXp1bCBkZWZpbmUgZWwgcHJveGltbyBlbiBwcm9kdWN0Ki99XG4gICAgICAgIDwvZGl2PlxuXG4gICAgICA8L2Rpdj5cbiAgICA8L21haW4+XG4gICAgey8qZm9vdGVyKi99XG4gICAgPC9kaXY+XG4gICk7XG59XG4iXSwibmFtZXMiOlsiUmVhY3QiLCJ1c2VTdGF0ZSIsInVzZUVmZmVjdCIsIk5hdmJhciIsIlN0b3JlUGFnZSIsIkhvbWUiLCJpbml0aWFsU3RhdGUiLCJpbXBWZW50YXMiLCJjYXJ0IiwicHJvZHVjdG9zIiwic3VidG90YWwiLCJ0b3RhbCIsImRpcmVjY2lvbkVudHJlZ2EiLCJtZXRvZG9zUGFnbyIsIm5lY2VzaXRhVmVyaWZpY2FjaW9uIiwidGllbmRhIiwic2V0VGllbmRhIiwic3RvcmVkVGllbmRhIiwibG9jYWxTdG9yYWdlIiwiZ2V0SXRlbSIsIkpTT04iLCJwYXJzZSIsInNldEl0ZW0iLCJzdHJpbmdpZnkiLCJoYW5kbGVDbGlja0FkZCIsIml0ZW0iLCJpc1ByZXNlbnQiLCJzb21lIiwicHJvZHVjdG8iLCJpZCIsIm51ZXZvc1Byb2R1Y3RvcyIsIm51ZXZvU3VidG90YWwiLCJyZWR1Y2UiLCJwcmVjaW8iLCJudWV2b1RvdGFsIiwiZGlyZWNjaW9uUyIsIm5lY2VzaXRhVmVyaWZpY2FjaW9uUyIsImRpdiIsInNpemUiLCJsZW5ndGgiLCJtYWluIiwiY2xhc3NOYW1lIl0sInNvdXJjZVJvb3QiOiIifQ==\n//# sourceURL=webpack-internal:///(app-pages-browser)/./src/app/page.js\n"));

/***/ })

});