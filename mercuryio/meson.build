mercuryio_lib = static_library(
    'mercuryio',
    name_prefix : '',
    include_directories : inc,
    implicit_include_directories : false,
    c_pch : '../precompiled.h',
    link_with : [
        util_lib,
    ],
    sources : [
        'mercuryio.c',
        'mercuryio.h',
        'config.c',
        'config.h',
    ],
)

shared_library(
    'mercuryio',
    name_prefix : '',
    vs_module_defs : 'mercuryio.def',
    link_with : [
        mercuryio_lib,
    ],
)