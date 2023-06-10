cmake_minimum_required (VERSION 3.8)

project ("smlite")

include_directories (.)
file (GLOB HEADERS smlite.hpp)
add_library(smlite ${HEADERS})
