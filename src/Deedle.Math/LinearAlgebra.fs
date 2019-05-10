﻿namespace Deedle.Math

open System.Runtime.CompilerServices
open MathNet.Numerics.LinearAlgebra
open Deedle
open Deedle.Math

module LinearAlgebra =

  let transpose (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> Matrix.transpose

  let inverse (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> Matrix.inverse
    
  let psudoInverse (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> fun x -> x.PseudoInverse()

  let conjugate (df:Frame<'R, 'C>) = 
    df
    |> Frame.toMatrix
    |> Matrix.conjugate

  let conjugateTranspose (df:Frame<'R, 'C>) = 
    df
    |> Frame.toMatrix
    |> Matrix.conjugateTranspose

  let norm (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> Matrix.norm
  
  let normRows (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> Matrix.normRows

  let normCols (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> Matrix.normCols

  let rank (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> Matrix.rank

  let trace (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> Matrix.trace

  let determinant (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> Matrix.determinant

  let condition (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> Matrix.condition

  let nullity (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> Matrix.nullity

  let kernel (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> Matrix.kernel

  let range (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> Matrix.range

  let isSymmetric (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> Matrix.symmetric

  let isHermitian (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> Matrix.hermitian

  let cholesky (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> Matrix.cholesky

  let lu (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> Matrix.lu

  let qr (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> Matrix.qr

  let svd (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> Matrix.svd

  let eigen (df:Frame<'R, 'C>) =
    df
    |> Frame.toMatrix
    |> Matrix.eigen

[<AutoOpen>]
module ``Frame Matrix Extensions`` =
  
  type Matrix =

    static member dot (df:Frame<'R, 'C>, m2:Matrix<float>) =
      let m1 = df |> Frame.toMatrix
      m1.Multiply(m2)

    static member dot (m1:Matrix<float>, df:Frame<'R, 'C>) =
      let m2 = df |> Frame.toMatrix
      m1.Multiply(m2)

    static member dot (s:Series<'K, float>, m2:Matrix<float>) =
      let m1 =
        if s.KeyCount = m2.ColumnCount then
          s.Values |> Array.ofSeq |> DenseMatrix.raw s.KeyCount 1
        elif s.KeyCount = m2.RowCount then
          s.Values |> Array.ofSeq |> DenseMatrix.raw 1 s.KeyCount
        else
          invalidOp "Mismatched Dimensions"
      m1.Multiply(m2)

    static member dot (m1:Matrix<float>, s:Series<'K, float>) =
      let m2 =
        if s.KeyCount = m1.ColumnCount then
          s.Values |> Array.ofSeq |> DenseMatrix.raw s.KeyCount 1
        elif s.KeyCount = m1.RowCount then
          s.Values |> Array.ofSeq |> DenseMatrix.raw 1 s.KeyCount
        else
          invalidOp "Mismatched Dimensions"
      m1.Multiply(m2)

    static member dot (df1:Frame<'R, 'C>, df2:Frame<'C, 'R>) =
      let m1 = df1 |> Frame.toMatrix
      // Align keys in the same order
      let m2 = df2.Rows.[df1.ColumnKeys]|> Frame.toMatrix
      m1.Multiply(m2)

    static member dot (df:Frame<'R, 'C>, s:Series<'C, float>) =
      let m1 = df |> Frame.toMatrix
      // Align keys in the same order. 
      // Create vector in the shape for matrix multiplication.
      let m2 =
        if m1.ColumnCount = s.KeyCount then
          s.[df.ColumnKeys].Values |> Array.ofSeq |> DenseMatrix.raw s.KeyCount 1
        elif s.KeyCount = m1.RowCount then
          s.[df.ColumnKeys].Values |> Array.ofSeq |> DenseMatrix.raw 1 s.KeyCount
        else
          invalidOp "Mismatched Dimensions"
      m1.Multiply(m2)

    static member dot (s:Series<'C, float>, df:Frame<'R, 'C>) =
      let m2 = df |> Frame.toMatrix
      // Align keys in the same order. 
      // Create vector in the shape for matrix multiplication.
      let m1 =
        if m2.ColumnCount = s.KeyCount then
          s.[df.ColumnKeys].Values |> Array.ofSeq |> DenseMatrix.raw 1 s.KeyCount
        elif m2.RowCount = s.KeyCount then
          s.[df.ColumnKeys].Values |> Array.ofSeq |> DenseMatrix.raw s.KeyCount 1
        else
          invalidOp "Mismatched Dimensions"
      m1.Multiply(m2)

    static member transpose (df:Frame<'R, 'C>) =
      df
      |> LinearAlgebra.transpose
      |> Frame.ofMatrix df.RowKeys df.ColumnKeys

    static member inverse (df:Frame<'R, 'C>) =
      df
      |> LinearAlgebra.inverse
      |> Frame.ofMatrix df.RowKeys df.ColumnKeys

    static member psudoInverse (df:Frame<'R, 'C>)=
      df
      |> LinearAlgebra.psudoInverse
      |> Frame.ofMatrix df.RowKeys df.ColumnKeys

    static member normRows (df:Frame<'R, 'C>) =
      df
      |> LinearAlgebra.normRows
      |> fun x -> Series(df.RowKeys, x.ToArray())

    static member normCols (df:Frame<'R, 'C>) =
      df
      |> LinearAlgebra.normCols
      |> fun x -> Series(df.ColumnKeys, x.ToArray())
    
[<Extension>]
type FrameExtensions =
  
  [<Extension>]
  static member Dot(df1:Frame<'R, 'C>, df:Frame<'C, 'R>) = 
    Matrix.dot(df1, df)

  [<Extension>]
  static member Dot(df:Frame<'R, 'C>, s:Series<'C, float>) = 
    Matrix.dot(df, s)

  [<Extension>]
  static member Dot(df:Frame<'R, 'C>, m:Matrix<float>) = 
    Matrix.dot(df, m)

[<Extension>]
type SeriesExtensions =

  [<Extension>]
  static member Dot(s:Series<'C, float>, df:Frame<'R, 'C>) = 
    Matrix.dot(s, df)

  [<Extension>]
  static member Dot(s:Series<'C, float>, m:Matrix<float>) = 
    Matrix.dot(s, m)

[<Extension>]
type MatrixExtensions =

  [<Extension>]
  static member Dot(m:Matrix<float>, df:Frame<'R, 'C>) = 
    Matrix.dot(m, df)

  [<Extension>]
  static member Dot(m1:Matrix<float>, m:Matrix<float>) = 
    m1.Multiply(m)

  [<Extension>]
  static member Dot(m1:Matrix<float>, v:Vector<float>) = 
    m1.Multiply(v)

  [<Extension>]
  static member Dot(m:Matrix<float>, s:Series<'K, float>) = 
    Matrix.dot(m, s)


